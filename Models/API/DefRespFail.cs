using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.API
{
    public class DefRespFail : ApiBase
    {

        public DefRespFail() { }
        public DefRespFail(string Message)
        {
            this.Messages.Add(Message);
        }
        public List<string> Messages = new();
    }
}