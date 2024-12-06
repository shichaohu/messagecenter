using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.Attributes;
using HS.Message.Share.AuditLog;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Utils;
using System.Reflection;
using System.Threading.Tasks;

namespace HS.Message.Service.@base
{
    /// <summary>
    /// 业务服务base接口
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TCondition"></typeparam>
    public abstract class BaseService<TModel, TCondition> : IBaseService<TModel, TCondition> where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
        /// <summary>
        /// 操作表
        /// </summary>
        protected readonly string _moduleName;
        protected readonly IInjectedObjects _injectedObjects;

        /// <summary>
        /// logical_id仓储操作类
        /// </summary>
        protected IRepository<TModel, TCondition> _baseRepository { get; }

        protected string[] _globalIgnoreField;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="moduleName">模块名称</param>
        protected BaseService(IRepository<TModel, TCondition> repository, IInjectedObjects injectedObjects, string moduleName)
        {
            _injectedObjects = injectedObjects;
            _baseRepository = repository;
            _moduleName = moduleName;

            _globalIgnoreField = new string[] { "queryFields", "orderby", "isNotLike", "fuzzySearchKeyWord", "fuzzySearchFields", "isLanguageHandle", "languageHandleEcludeField", "limitNum" };
        }

        /// <summary>
        /// 检查TModel数据有效性
        /// </summary>
        /// <param name="model">待检查数据模型</param>
        /// <returns>检查结果</returns>
        public BaseResponse CheckInfoBase(TModel model, TModel modelOld = null)
        {
            // 操作结果
            BaseResponse returnResultBase = CheckInfo(model, modelOld);
            if (returnResultBase.Code != ResponseCode.Success)
            {
                return returnResultBase;
            }

            // 自定义数据校验

            returnResultBase.Code = ResponseCode.Success;
            return returnResultBase;
        }

        /// <summary>
        /// 检查TModel数据有效性
        /// </summary>
        /// <param name="model">待检查数据模型</param>
        /// <returns>检查结果</returns>
        public virtual BaseResponse CheckInfo(TModel model, TModel modelOld)
        {
            BaseResponse result = new() { Code = ResponseCode.Success };

            return result;

        }

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<long> GetTotalCountAsync(TCondition condition)
        {
            return await _baseRepository.GetTotalCountAsync(condition);
        }

        /// <summary>
        /// 根据动态模型转换为详细的数据模型
        /// </summary>
        /// <param name="modelJobj">动态编辑模型</param>
        /// <param name="modelOld">历史数据模型</param>
        /// <returns>转换后的实体模型</returns>
        protected virtual BaseResponse TransformatJobjToMode(Dictionary<string, object> modelJobj, ref TModel model, ref TModel modelOld)
        {
            // 处理结果
            BaseResponse mReturnResultBase = new() { Code = ResponseCode.InternalError };

            string[] ignoreFields = new string[] { "id", "logical_id", "created_id", "created_name", "CreatedTime" };
            var fieldsDic = GetAllFields(ignoreFields);
            if (fieldsDic?.Keys.Count == 0)
            {
                mReturnResultBase.Message = $"there is no field in model:{typeof(TModel).Name}";
                return mReturnResultBase;
            }

            if (modelJobj != null && modelJobj.Keys.Count > 0)
            {
                #region 第一步：首先是要获取主键字段,如果没有传递主键字段，那么直接返回失败

                // 获取字典对应的所有键
                List<string> inputKeyList = modelJobj.Keys.ToList();

                // 当前操作的键，在获取键的时候，所有都不区分大小写
                string thisKey = string.Empty;

                // 操作字典是否传递了主键值
                thisKey = inputKeyList.Find(x => x.Equals("logical_id", StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrEmpty(thisKey))
                {

                    // 根据Id获取对于的数据库实体模型，如果没有获取到数据模型，说明id有误
                    model = _baseRepository.GetModelById(modelJobj[thisKey].ToString());
                    if (model == null)
                    {
                        mReturnResultBase.Message = "系统找不到操作的数据！";
                        return mReturnResultBase;
                    }

                    // 组装信息数据，深拷贝
                    modelOld = JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(model));
                }
                else
                {
                    mReturnResultBase.Message = "请选择需要操作的数据！";
                    return mReturnResultBase;
                }

                #endregion

                // 第二步：根据实际需要组装新增后的数据
                foreach (var field in fieldsDic)
                {
                    Type modelType = model.GetType();
                    string currentKey = inputKeyList.FirstOrDefault(x => x.Equals(field.Key, StringComparison.CurrentCultureIgnoreCase));

                    var value = !string.IsNullOrWhiteSpace(currentKey) ? modelJobj[currentKey]?.ToString() : null;

                    if (value != null)
                    {
                        if (field.Value == typeof(string))
                        {
                            modelType.GetProperty(field.Key).SetValue(model, value);
                        }
                        else if (field.Value == typeof(DateTime))
                        {
                            if (DateTime.TryParse(value, out DateTime val) && val.IsEffectiveDateTime())
                            {
                                modelType.GetProperty(field.Key).SetValue(model, val);
                            }
                        }
                        else if (field.Value.IsValueType)
                        {
                            if (field.Value == typeof(int))
                                modelType.GetProperty(field.Key).SetValue(model, int.Parse(value));
                            else if (field.Value == typeof(float))
                                modelType.GetProperty(field.Key).SetValue(model, float.Parse(value));
                            else if (field.Value == typeof(double))
                                modelType.GetProperty(field.Key).SetValue(model, double.Parse(value));
                            else if (field.Value == typeof(decimal))
                                modelType.GetProperty(field.Key).SetValue(model, decimal.Parse(value));
                        }
                    }

                }


            }
            else
            {
                mReturnResultBase.Message = "基本信息不能为空！";
                return mReturnResultBase;
            }

            mReturnResultBase.Code = ResponseCode.Success;

            return mReturnResultBase;
        }
        /// <summary>
        /// 获取表全部字段
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, Type> GetAllFields(string[] ignoreField = null)
        {
            Type mType = typeof(TModel);
            Dictionary<string, Type> fieldMap = new();
            ignoreField = ignoreField ?? Array.Empty<string>();
            foreach (var propInfo in mType.GetProperties())
            {
                var attr = propInfo.GetCustomAttribute<FieldAttribute>();
                if (attr != null)
                {
                    if (propInfo.PropertyType.IsPublic
                        && !_globalIgnoreField.Contains(propInfo.Name)
                        && !ignoreField.Contains(propInfo.Name))
                    {
                        fieldMap.TryAdd(propInfo.Name, propInfo.PropertyType);
                    }
                }
            }

            return fieldMap;
        }

        /// <summary>
        /// 新增一条Test记录
        /// </summary>
        /// <param name="model">Test数据模型</param>
        /// <returns>操作结果</returns>
        public async Task<BaseResponse> AddOneAsync(TModel model)
        {
            // 操作结果
            BaseResponse returnResultBase = new()
            { Code = ResponseCode.Success };
            // 第一步：数据校验
            returnResultBase = CheckInfoBase(model);
            if (returnResultBase.Code != ResponseCode.Success)
            {
                return returnResultBase;
            }

            //  第二步：数据落地
            returnResultBase.Code = await _baseRepository.AddOneAsync(model) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            returnResultBase.Message = returnResultBase.Code == ResponseCode.Success ? "添加成功！" : "添加失败！";

            return returnResultBase;
        }

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>操作结果</returns>
        public async Task<BaseResponse> BactchAddAsync(List<TModel> modelList)
        {
            // 操作结果
            BaseResponse returnResultBase = new()
            { Code = ResponseCode.Success };

            // 第一步：数据校验
            if (modelList == null || modelList.Count < 1)
            {
                returnResultBase = new BaseResponse()
                {
                    Code = ResponseCode.ParameterError,
                    Message = "基本信息不能为空！"
                };

                return returnResultBase;
            }

            foreach (var model in modelList)
            {
                returnResultBase = CheckInfoBase(model);
                if (returnResultBase.Code != ResponseCode.Success)
                {
                    return returnResultBase;
                }
            }

            //  第二步：数据落地
            returnResultBase.Code = await _baseRepository.BactchAddAsync(modelList) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            returnResultBase.Message = returnResultBase.Code == ResponseCode.Success ? "添加成功！" : "添加失败！";

            return returnResultBase;
        }

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public async Task<BaseResponse> DeleteByIdAsync(string logical_id)
        {
            // 操作结果
            BaseResponse returnResultBase = new()
            { Code = ResponseCode.Success };

            // 第一步：数据校验
            if (string.IsNullOrEmpty(logical_id))
            {
                returnResultBase.Message = "请选择要删除的数据记录！";
                returnResultBase.Code = ResponseCode.ParameterError;

                return returnResultBase;
            }

            // 获取原始数据模型
            TModel model = await _baseRepository.GetModelByIdAsync(logical_id);

            if (model == null)
            {
                returnResultBase.Message = "数据记录不存在！";
                returnResultBase.Code = ResponseCode.ParameterError;

                return returnResultBase;
            }

            //  第二步：数据落地
            returnResultBase.Code = await _baseRepository.DeleteByIdAsync(logical_id) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            returnResultBase.Message = returnResultBase.Code == ResponseCode.Success ? "删除成功！" : "删除失败！";

            return returnResultBase;
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public async Task<BaseResponse> BactchDeleteByIdListAsync(List<string> idList)
        {
            // 操作结果
            BaseResponse returnResultBase = new()
            { Code = ResponseCode.Success };

            // 第一步：数据校验
            if (idList == null || idList.Count <= 0)
            {
                returnResultBase.Message = "请选择要删除的数据记录！";
                returnResultBase.Code = ResponseCode.ParameterError;

                return returnResultBase;
            }

            //  第二步：数据落地
            returnResultBase.Code = await _baseRepository.BactchDeleteByIdListAsync(idList) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            returnResultBase.Message = returnResultBase.Code == ResponseCode.Success ? "删除成功！" : "删除失败！";

            return returnResultBase;
        }

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public async Task<BaseResponse> UpdateByIdAsync(TModel model)
        {
            // 操作结果
            BaseResponse mReturnResultBase = new()
            { Code = ResponseCode.Success };

            //// 第一步：数据校验
            if (model == null)
            {
                mReturnResultBase.Message = "Test基本信息不能为空！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            // 获取一下原始数据
            TModel modelOld = await _baseRepository.GetModelByIdAsync(model.LogicalId);

            if (modelOld == null)
            {
                mReturnResultBase.Message = "数据记录不存在！";
                mReturnResultBase.Code = ResponseCode.ParameterError;

                return mReturnResultBase;
            }

            mReturnResultBase = CheckInfoBase(model, modelOld);
            if (mReturnResultBase.Code != ResponseCode.Success)
            {
                return mReturnResultBase;
            }

            //// 第二步，执行更新
            mReturnResultBase.Code = await _baseRepository.UpdateByIdAsync(model) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            mReturnResultBase.Message = mReturnResultBase.Code == ResponseCode.Success ? "更新成功！" : "更新失败！";

            return mReturnResultBase;
        }

        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        public async Task<BaseResponse> BactchUpdateByIdAsync(List<TModel> modelList)
        {
            // 操作结果
            BaseResponse mReturnResultBase = new()
            { Code = ResponseCode.Success };

            //// 第一步：数据校验
            if (modelList == null || modelList.Count < 1)
            {
                mReturnResultBase.Message = "Auth基本信息不能为空！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            // 获取一下原始数据
            List<TModel> modelListOld = await _baseRepository.GetAllListByIdListAsync(modelList.Select(x => x.LogicalId).ToList());
            if (modelListOld == null && modelListOld.Count != modelList.Count)
            {
                mReturnResultBase.Message = "所提交的数据集合与系统数据不匹配！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            // 日志变更
            StringBuilder audit_value = new StringBuilder();

            // 依次对每一个数据做校验
            foreach (var model in modelList)
            {
                TModel modelOld = modelListOld.Find(x => x.LogicalId == model.LogicalId);

                if (modelOld == null)
                {
                    mReturnResultBase.Message = "数据记录不存在！";
                    mReturnResultBase.Code = ResponseCode.ParameterError;

                    return mReturnResultBase;
                }

                mReturnResultBase = CheckInfoBase(model, modelOld);
                if (mReturnResultBase.Code != ResponseCode.Success)
                {
                    return mReturnResultBase;
                }

                audit_value.AppendLine(new AuditLog<TModel>().GeteChangeAuditLog(
                    JsonConvert.DeserializeObject<TModel>(
                        JsonConvert.SerializeObject(modelOld, PublicTools.GetDateTimeConverter())
                    ),
                    model));

                audit_value.Append("；");
            }

            //// 第二步，执行更新
            mReturnResultBase.Code = await _baseRepository.BactchUpdateByIdAsync(modelList) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            mReturnResultBase.Message = mReturnResultBase.Code == ResponseCode.Success ? "更新成功！" : "更新失败！";

            return mReturnResultBase;
        }

        /// <summary>
        /// 动态更新，根据实际需要，传递什么字段就更新什么字段信息
        /// </summary>
        /// <param name="modelJobj">需要更新的 test 数据Jobject实例</param>
        /// <returns>处理结果</returns>
        public async Task<BaseResponse> UpdateDynamicAsync(Dictionary<string, object> modelJobj)
        {
            // 数据模型
            TModel model = null;

            // 第一步：根据变更的动态模型+数据库实例数据，转换为最终的数据模型
            // 获取一下原始数据
            TModel modelOld = null;
            BaseResponse mReturnResultBase = TransformatJobjToMode(modelJobj, ref model, ref modelOld);

            // 第二步：如果数据转换成功，那么做一个数据校验处理
            if (mReturnResultBase != null && mReturnResultBase.Code == ResponseCode.Success)
            {
                mReturnResultBase = CheckInfoBase(model, modelOld);
            }

            // 第三步：如果校验成功，那么做数据更新处理，失败直接返回
            if (mReturnResultBase != null && mReturnResultBase.Code == ResponseCode.Success)
            {

                mReturnResultBase.Code = await _baseRepository.UpdateByIdAsync(model) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
                mReturnResultBase.Message = mReturnResultBase.Code == ResponseCode.Success ? "更新成功！" : "更新失败！";

            }
            else
            {
                return mReturnResultBase ?? new BaseResponse() { Code = ResponseCode.InternalError, Message = "操作失败！" };
            }

            return mReturnResultBase;
        }

        /// <summary>
        /// 动态更新，根据实际需要，传递什么字段就更新什么字段信息（批量更新）
        /// </summary>
        /// <param name="modelJobjList">需要更新的 auth 数据Jobject实例</param>
        /// <returns>处理结果</returns>
        public async Task<BaseResponse> BactchUpdateDynamicAsync(List<Dictionary<string, object>> modelJobjList)
        {
            BaseResponse mReturnResultBase = new BaseResponse()
            { Code = ResponseCode.Success };

            // 数据模型
            List<TModel> modelList = new List<TModel>();
            List<TModel> modelListOld = new List<TModel>();

            // 第一步：根据变更的动态模型+数据库实例数据，转换为最终的数据模型
            // 日志变更
            StringBuilder audit_value = new StringBuilder();

            foreach (var item in modelJobjList)
            {
                TModel modelOne = null;
                TModel modelOld = null;

                mReturnResultBase = TransformatJobjToMode(item, ref modelOne, ref modelOld);

                // 如果数据转换成功，那么做一个数据校验处理
                if (mReturnResultBase != null && mReturnResultBase.Code == ResponseCode.Success)
                {
                    mReturnResultBase = CheckInfoBase(modelOne, modelOld);
                }

                if (mReturnResultBase.Code != ResponseCode.Success)
                {
                    return mReturnResultBase;
                }

                audit_value.AppendLine(new AuditLog<TModel>().GeteChangeAuditLog(
                   JsonConvert.DeserializeObject<TModel>(
                       JsonConvert.SerializeObject(modelOld, PublicTools.GetDateTimeConverter())
                   ), modelOne));

                modelList.Add(modelOne);
                modelListOld.Add(modelOld);
            }

            mReturnResultBase.Code = await _baseRepository.BactchUpdateByIdAsync(modelList) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            mReturnResultBase.Message = mReturnResultBase.Code == ResponseCode.Success ? "更新成功！" : "更新失败！";


            return mReturnResultBase;
        }

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="bactchUpdateSpecifyFields">logical_id集合</param>
        /// <returns>所有数据集合</returns>
        public async Task<BaseResponse> BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            // 操作结果
            BaseResponse mReturnResultBase = new BaseResponse()
            { Code = ResponseCode.Success };

            //// 第一步：数据校验
            if (bactchUpdateSpecifyFields == null || bactchUpdateSpecifyFields.idList == null || bactchUpdateSpecifyFields.idList.Count < 1 ||
                bactchUpdateSpecifyFields.updateFieldsValue == null || bactchUpdateSpecifyFields.updateFieldsValue.Count < 1)
            {
                mReturnResultBase.Message = "基本信息不能为空！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            //// 第二步，执行更新
            mReturnResultBase.Code = await _baseRepository.BactchUpdateSpecifyFieldsByIdAsync(bactchUpdateSpecifyFields) > 0 ? ResponseCode.Success : ResponseCode.InternalError;
            mReturnResultBase.Message = mReturnResultBase.Code == ResponseCode.Success ? "更新成功！" : "更新失败！";


            return mReturnResultBase;
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        public async Task<BaseResponse<TModel>> GetModelByIdAsync(string logical_id, string queryFields = "")
        {
            // 操作结果
            BaseResponse<TModel> mReturnResultBase = new BaseResponse<TModel>()
            { Code = ResponseCode.Success };

            if (string.IsNullOrEmpty(logical_id))
            {
                mReturnResultBase.Message = "需要获取的数据logical_id不能为空！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            // 第二步：直接获取数据
            mReturnResultBase.Data = await _baseRepository.GetModelByIdAsync(logical_id, queryFields);

            return mReturnResultBase;
        }

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        public async Task<BaseResponse<TModel>> GetOneModelAsync(TCondition condition)
        {
            // 操作结果
            BaseResponse<TModel> mReturnResultBase = new BaseResponse<TModel>()
            { Code = ResponseCode.Success };

            if (condition == null)
            {
                mReturnResultBase.Message = "需要获取的信息不能为空！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            // 第二步：直接获取数据
            mReturnResultBase.Data = await _baseRepository.GetOneModelAsync(condition);

            return mReturnResultBase;
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public async Task<BaseResponse<List<TModel>>> GetAllListByIdListAsync(List<string> idList, string queryFields = "")
        {
            // 操作结果
            BaseResponse<List<TModel>> mReturnResultBase = new BaseResponse<List<TModel>>()
            { Code = ResponseCode.Success };

            if (idList == null || idList.Count < 1)
            {
                mReturnResultBase.Message = "需要获取的信息不能为空！";
                mReturnResultBase.Code = ResponseCode.ParameterError;
                return mReturnResultBase;
            }

            // 第二步：直接获取数据
            mReturnResultBase.Data = await _baseRepository.GetAllListByIdListAsync(idList, queryFields);

            return mReturnResultBase;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="mPageQueryCondition">查询条件</param>
        /// <param name="islike">是否进行liek查询，默认是false</param>
        /// <returns>符合要求的分页数据集合</returns>
        public async Task<MPageQueryResponse<TModel>> GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition)
        {
            //// 操作结果
            MPageQueryResponse<TModel> returnResultBase = new MPageQueryResponse<TModel>()
            { Code = ResponseCode.Success };

            //// 直接获取数据
            returnResultBase.Data = await _baseRepository.GetPageListAsync(pageQueryCondition);

            //// 分页数据赋值
            returnResultBase.PageIndex = pageQueryCondition.pageIndex;
            returnResultBase.PageSize = pageQueryCondition.pageSize;

            //// 符合要求的数据总条数
            returnResultBase.Total = pageQueryCondition.total;

            return returnResultBase;
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合要求的数据集合</returns>
        public async Task<MPageQueryResponse<TModel>> GetAllListAsync(TCondition condition)
        {
            //// 操作结果
            MPageQueryResponse<TModel> returnResultBase = new MPageQueryResponse<TModel>()
            { Code = ResponseCode.Success };

            //// 直接获取数据
            returnResultBase.Data = await _baseRepository.GetAllListAsync(condition, limitNum: condition != null ? condition.LimitNum : 0);

            return returnResultBase;
        }


    }
}
