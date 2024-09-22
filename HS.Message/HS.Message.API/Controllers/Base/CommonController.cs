using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Enums;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers.Base
{
    /// <summary>
    /// 公共方法的Controller
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TCondition"></typeparam>
    public class CommonController<TModel, TCondition> : ControllerBase where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {

        protected const string excleContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected const string excleDirectory = "excleUpload";

        private readonly IBaseService<TModel, TCondition> _baseService;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="baseService"></param>
        public CommonController(IBaseService<TModel, TCondition> baseService)
        {
            _baseService = baseService;
        }


        #region 公共接口
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        [Route("AddOne")]
        [HttpPost]
        public async Task<BaseResponse> AddOneAsync(TModel model)
        {
            return await _baseService.AddOneAsync(model);
        }

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        [Route("BactchAdd")]
        [HttpPost]
        public async Task<BaseResponse> BactchAddAsync(List<TModel> modelList)
        {
            return await _baseService.BactchAddAsync(modelList);
        }

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>影响的行数</returns>
        [HttpGet("DeleteById/{id}")]
        public async Task<BaseResponse> DeleteByIdAsync(string id)
        {
            return await _baseService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 根据Id集合批量删除数据
        /// </summary>
        /// <param name="idList">Id集合</param>
        /// <returns>处理结果</returns>
        [Route("BactchDeleteByIdList")]
        [HttpPost]
        public async Task<BaseResponse> BactchDeleteByIdListAsync(List<string> idList)
        {
            return await _baseService.BactchDeleteByIdListAsync(idList);
        }

        /// <summary>
        /// 根据id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        [Route("UpdateById")]
        [HttpPost]
        public async Task<BaseResponse> UpdateByIdAsync(TModel model)
        {
            return await _baseService.UpdateByIdAsync(model);
        }

        /// <summary>
        /// 根据id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        [Route("BactchUpdateById")]
        [HttpPost]
        public async Task<BaseResponse> BactchUpdateByIdAsync(List<TModel> modelList)
        {
            return await _baseService.BactchUpdateByIdAsync(modelList);
        }

        /// <summary>
        /// 动态更新，根据实际需要，传递什么字段就更新什么字段信息
        /// </summary>
        /// <param name="modelJobj">需要更新的 Dictionary 数据Jobject实例</param>
        /// <returns>处理结果</returns>
        [Route("UpdateDynamic")]
        [HttpPost]
        public async Task<BaseResponse> UpdateDynamicAsync(Dictionary<string, object> modelJobj)
        {
            return await _baseService.UpdateDynamicAsync(modelJobj);
        }

        /// <summary>
        /// 动态更新，根据实际需要，传递什么字段就更新什么字段信息（批量更新）
        /// </summary>
        /// <param name="modelJobjList">需要更新的 auth 数据Jobject实例</param>
        /// <returns>处理结果</returns>
        [Route("BactchUpdateDynamic")]
        [HttpPost]
        public async Task<BaseResponse> BactchUpdateDynamicAsync(List<Dictionary<string, object>> modelJobjList)
        {
            return await _baseService.BactchUpdateDynamicAsync(modelJobjList);
        }

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="bactchUpdateSpecifyFields">id集合</param>
        /// <returns>所有数据集合</returns>
        [Route("BactchUpdateSpecifyFieldsById")]
        [HttpPost]
        public async Task<BaseResponse> BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            return await _baseService.BactchUpdateSpecifyFieldsByIdAsync(bactchUpdateSpecifyFields);
        }

        /// <summary>
        /// 根据id获取一个模型数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        [HttpGet("GetModelById/{id}")]
        [HttpGet("GetModelById/{id}/{queryFields}")]
        public async Task<BaseResponse<TModel>> GetModelByIdAsync(string id, string queryFields = "")
        {
            return await _baseService.GetModelByIdAsync(id, queryFields);
        }

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        [Route("GetOneModel")]
        [HttpPost]
        public async Task<BaseResponse<TModel>> GetOneModelAsync(TCondition condition)
        {
            return await _baseService.GetOneModelAsync(condition);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        [Route("GetAllListByIdList")]
        [Route("GetAllListByIdList/{queryFields}")]
        [HttpPost]
        public async Task<BaseResponse<List<TModel>>> GetAllListByIdListAsync(List<string> idList, string queryFields = "")
        {
            return await _baseService.GetAllListByIdListAsync(idList, queryFields);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        [Route("GetPageList")]
        [HttpPost]
        public async Task<MPageQueryResponse<TModel>> GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition)
        {
            if (pageQueryCondition.condition == null)
            {
                pageQueryCondition.condition = new TCondition();
            }

            return await _baseService.GetPageListAsync(pageQueryCondition);
        }

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合要求的全部数据集合</returns>
        [Route("GetAllList")]
        [HttpPost]
        public async Task<MPageQueryResponse<TModel>> GetAllListAsync(TCondition condition)
        {
            return await _baseService.GetAllListAsync(condition);
        }


        #endregion
    }
}
