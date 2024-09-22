using Serilog.Debugging;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Log.Serilogs.LogEventSinks
{
    internal abstract class BatchProvider : IDisposable
    {
        private const int MaxSupportedBufferSize = 100000;

        private const int MaxSupportedBatchSize = 1000;

        private int _numMessages;

        private bool _canStop;

        private readonly int _maxBufferSize;

        private readonly int _batchSize;

        private readonly ConcurrentQueue<LogEvent> _logEventBatch;

        private readonly BlockingCollection<IList<LogEvent>> _batchEventsCollection;

        private readonly BlockingCollection<LogEvent> _eventsCollection;

        private readonly TimeSpan _timerThresholdSpan = TimeSpan.FromSeconds(10.0);

        private readonly TimeSpan _transientThresholdSpan = TimeSpan.FromSeconds(5.0);

        private readonly Task _timerTask;

        private readonly Task _batchTask;

        private readonly Task _eventPumpTask;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly AutoResetEvent _timerResetEvent = new AutoResetEvent(initialState: false);

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private bool _disposedValue;

        protected BatchProvider(int batchSize = 100, int maxBufferSize = 25000)
        {
            _maxBufferSize = Math.Min(Math.Max(5000, maxBufferSize), 100000);
            _batchSize = Math.Min(Math.Max(batchSize, 1), 1000);
            _logEventBatch = new ConcurrentQueue<LogEvent>();
            _batchEventsCollection = new BlockingCollection<IList<LogEvent>>();
            _eventsCollection = new BlockingCollection<LogEvent>(maxBufferSize);
            _batchTask = Task.Factory.StartNew(new Func<Task>(PumpAsync), TaskCreationOptions.LongRunning);
            _timerTask = Task.Factory.StartNew(new Action(TimerPump), TaskCreationOptions.LongRunning);
            _eventPumpTask = Task.Factory.StartNew(new Action(EventPump), TaskCreationOptions.LongRunning);
        }

        private async Task PumpAsync()
        {
            try
            {
                while (!_batchEventsCollection.IsCompleted)
                {
                    IList<LogEvent> logEvents = _batchEventsCollection.Take(_cancellationTokenSource.Token);
                    SelfLog.WriteLine($"Sending batch of {logEvents.Count} logs");
                    if (await WriteLogEventAsync(logEvents).ConfigureAwait(continueOnCapturedContext: false))
                    {
                        Interlocked.Add(ref _numMessages, -1 * logEvents.Count);
                    }
                    else
                    {
                        TimeSpan transientThresholdSpan = _transientThresholdSpan;
                        SelfLog.WriteLine($"Retrying after {transientThresholdSpan.TotalSeconds} seconds...");
                        await Task.Delay(_transientThresholdSpan).ConfigureAwait(continueOnCapturedContext: false);
                        if (!_batchEventsCollection.IsAddingCompleted)
                        {
                            _batchEventsCollection.Add(logEvents);
                        }
                    }

                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex3)
            {
                SelfLog.WriteLine(ex3.Message);
            }
        }

        private void TimerPump()
        {
            while (!_canStop)
            {
                _timerResetEvent.WaitOne(_timerThresholdSpan);
                FlushLogEventBatch();
            }
        }

        private void EventPump()
        {
            try
            {
                while (!_eventsCollection.IsCompleted)
                {
                    LogEvent item = _eventsCollection.Take(_cancellationTokenSource.Token);
                    _logEventBatch.Enqueue(item);
                    if (_logEventBatch.Count >= _batchSize)
                    {
                        FlushLogEventBatch();
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex3)
            {
                SelfLog.WriteLine(ex3.Message);
            }
        }

        private void FlushLogEventBatch()
        {
            try
            {
                _semaphoreSlim.Wait(_cancellationTokenSource.Token);
                if (!_logEventBatch.Any())
                {
                    return;
                }

                int num = ((_logEventBatch.Count >= _batchSize) ? _batchSize : _logEventBatch.Count);
                List<LogEvent> list = new List<LogEvent>();
                for (int i = 0; i < num; i++)
                {
                    if (_logEventBatch.TryDequeue(out var result))
                    {
                        list.Add(result);
                    }
                }

                if (!_batchEventsCollection.IsAddingCompleted)
                {
                    _batchEventsCollection.Add(list);
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _semaphoreSlim.Release();
                }
            }
        }

        protected void PushEvent(LogEvent logEvent)
        {
            if (_numMessages <= _maxBufferSize && !_eventsCollection.IsAddingCompleted)
            {
                _eventsCollection.Add(logEvent);
                Interlocked.Increment(ref _numMessages);
            }
        }

        protected abstract Task<bool> WriteLogEventAsync(ICollection<LogEvent> logEventsBatch);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    FlushAndCloseEventHandlers();
                    _semaphoreSlim.Dispose();
                    SelfLog.WriteLine("Sink halted successfully.");
                }

                _disposedValue = true;
            }
        }

        private void FlushAndCloseEventHandlers()
        {
            try
            {
                SelfLog.WriteLine("Halting sink...");
                _canStop = true;
                _timerResetEvent.Set();
                _eventsCollection.CompleteAdding();
                while (!_eventsCollection.IsCompleted)
                {
                    LogEvent item = _eventsCollection.Take();
                    _logEventBatch.Enqueue(item);
                    if (_logEventBatch.Count >= _batchSize)
                    {
                        FlushLogEventBatch();
                    }
                }

                FlushLogEventBatch();
                _batchEventsCollection.CompleteAdding();
                _cancellationTokenSource.Cancel();
                while (!_batchEventsCollection.IsCompleted)
                {
                    IList<LogEvent> list = _batchEventsCollection.Take();
                    WriteLogEventAsync(list).GetAwaiter().GetResult();
                    SelfLog.WriteLine($"Sending batch of {list.Count} logs");
                }

                Task.WaitAll(new Task[3] { _eventPumpTask, _batchTask, _timerTask }, TimeSpan.FromSeconds(60.0));
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
