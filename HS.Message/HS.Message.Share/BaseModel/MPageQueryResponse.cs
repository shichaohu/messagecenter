﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MPageQueryResponse<T>:BaseResponse
    {
        public List<T> Data { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}