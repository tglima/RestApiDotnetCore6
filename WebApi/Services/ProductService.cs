using WebApi.Helpers;
using WebApi.Models.API;
using WebApi.Models.DTO;
using WebApi.Models.Examples;

namespace WebApi.Services
{
    public class ProductService
    {
        private readonly LogService _logService;

        public ProductService(LogService logService)
        {
            this._logService = logService;
        }

        public ReturnDTO FindProductById(string id)
        {
            var returnDTO = new ReturnDTO("FindProductById");
            var productExample = new ProductExample();

            var idProduct = AppHelper.ToNullableInt(id);

            returnDTO.StatusCode = StatusCodeApi.FAIL;
            returnDTO.Returnbject = new DefaultResponse(Constant.MsgStatus400, this._logService.LogDTO.CodeEvent);

            if (idProduct != null)
            {
                returnDTO.StatusCode = StatusCodeApi.SUCCESS;
                returnDTO.Returnbject = productExample.GetExamples();
            }

            return returnDTO;
        }

        public ReturnDTO FindProducts()
        {
            var returnDTO = new ReturnDTO("FindProducts");
            var arrayProductExample = new ArrayProductExample();

            returnDTO.StatusCode = StatusCodeApi.SUCCESS;
            returnDTO.Returnbject = arrayProductExample.GetExamples();
            return returnDTO;
        }


    }
}