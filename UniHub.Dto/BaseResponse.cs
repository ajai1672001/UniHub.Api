using System.Net;

namespace UniHub.Dto
{
    public class BaseResponse<T> where T : class
    {
        public HttpStatusCode Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public T? Data { get; set; }

        public BaseResponse()
        { }

        public BaseResponse(bool isSuccess)
        {
            Code = isSuccess ? HttpStatusCode.OK : HttpStatusCode.UnprocessableEntity;
            IsSuccess = isSuccess;
            Message = string.Empty;
            Data = default;
        }

        public BaseResponse(T data)
        {
            Code = HttpStatusCode.OK;
            IsSuccess = true;
            Message = string.Empty;
            Data = data;
        }

        public BaseResponse(bool isSuccess, T data)
        {
            Code = isSuccess ? HttpStatusCode.OK : HttpStatusCode.UnprocessableEntity;
            IsSuccess = isSuccess;
            Message = string.Empty;
            Data = data;
        }

        public BaseResponse(bool isSuccess, T data, string message)
        {
            Code = isSuccess ? HttpStatusCode.OK : HttpStatusCode.UnprocessableEntity;
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public BaseResponse(bool isSuccess, T data, string message, HttpStatusCode statusCode)
        {
            Code = statusCode;
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
    }
}