using Core.Utilities;

namespace Shared
{
    class ActionResponse
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; }
        public object Data { get; set; }
        public bool IsError
        {
            get => StatusCode != 200;
        }

        public ActionResponse(object data = null, string message = "", int statusCode = 200)
        {
            this.StatusCode = statusCode;
            this.Message = message == string.Empty ? (statusCode == 200 ? MessageBuilder.Success : MessageBuilder.Fail) : message;
            this.Data = data;
        }
        public ActionResponse(string message = "", int statusCode = 200)
        {
            this.StatusCode = statusCode;
            this.Message = message == string.Empty ? (statusCode == 200 ? MessageBuilder.Success : MessageBuilder.Fail) : message;
        }
        public ActionResponse(object data = null, int statusCode = 200)
        {
            this.StatusCode = statusCode;
            this.Message = statusCode == 200 ? MessageBuilder.Success : MessageBuilder.Fail;
            this.Data = data;
        }

        public ActionResponse(object data = null)
        {
            this.Message = MessageBuilder.Success;
            this.Data = data;
        }
        public ActionResponse()
        {
            this.Message = MessageBuilder.Success;
        }

    }

}
