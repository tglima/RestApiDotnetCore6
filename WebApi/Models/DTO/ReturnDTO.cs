using Microsoft.AspNetCore.Mvc;
using WebApi.Models.API;

namespace WebApi.Models.DTO
{
    public class ReturnDTO
    {

        public ReturnDTO() { }

        public ReturnDTO(string NmMethod)
        {
            this.NmMethod = NmMethod;
        }

        public StatusCodeApi StatusCode;
        public object? Returnbject;
        public string NmMethod = string.Empty;
        public List<string> Info = new();
        public List<ReturnDTO> Methods = new();

        public static ActionResult ToActionResult(ReturnDTO returnDTO)
        {
            return new ObjectResult(returnDTO.Returnbject)
            {
                StatusCode = Convert.ToInt32(returnDTO.StatusCode)
            };
        }
    }
}