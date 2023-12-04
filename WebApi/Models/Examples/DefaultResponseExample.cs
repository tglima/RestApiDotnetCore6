using WebApi.Helpers;
using WebApi.Models.API;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Models.Examples
{

    public class InvalidRequestExample : IExamplesProvider<DefaultResponse>
    {
        public DefaultResponse GetExamples()
        {
            return new DefaultResponse(Constant.MsgStatus400, "b000b00a-ab00-0000-b000-bb000b000000");

        }
    }

    public class AuthFailedExample : IExamplesProvider<DefaultResponse>
    {
        public DefaultResponse GetExamples()
        {
            return new DefaultResponse(Constant.MsgStatus401, "b000b00a-ab00-0000-b000-bb000b000000");

        }
    }

    public class NotFoundExample : IExamplesProvider<DefaultResponse>
    {
        public DefaultResponse GetExamples()
        {
            return new DefaultResponse(Constant.MsgStatus404, "b000b00a-ab00-0000-b000-bb000b000000");

        }
    }

    public class ServerErrorExample : IExamplesProvider<DefaultResponse>
    {
        public DefaultResponse GetExamples()
        {
            return new DefaultResponse(Constant.MsgStatus500, "b000b00a-ab00-0000-b000-bb000b000000");

        }
    }

}