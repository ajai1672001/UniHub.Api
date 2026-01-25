using System.Net;

namespace UniHub.Dto
{
    public class BaseResponse<T>
    {
        public HttpStatusCode Code { get; private set; }
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T? Data { get; private set; }

        private BaseResponse() { }

        private BaseResponse(HttpStatusCode code, bool isSuccess, string message, T? data)
        {
            Code = code;
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        #region Success Responses

        public static BaseResponse<T> Success( T? data = default)
        {
            return new BaseResponse<T>(HttpStatusCode.OK, true, "", data);
        }

        public static BaseResponse<T> Created(string message = "", T? data = default)
        {
            return new BaseResponse<T>(HttpStatusCode.Created, true, message, data);
        }

        public static BaseResponse<T> NoContent(string message = "")
        {
            return new BaseResponse<T>(HttpStatusCode.NoContent, true, message, default);
        }

        #endregion

        #region Failure Responses

        public static BaseResponse<T> Fail(string message, HttpStatusCode code = HttpStatusCode.UnprocessableEntity, T data = default)
        {
            return new BaseResponse<T>(code, false, message, data);
        }

        public static BaseResponse<T> NotFound(string message = "Not found")
        {
            return new BaseResponse<T>(HttpStatusCode.NotFound, false, message, default);
        }

        public static BaseResponse<T> Unauthorized(string message = "Unauthorized")
        {
            return new BaseResponse<T>(HttpStatusCode.Unauthorized, false, message, default);
        }

        public static BaseResponse<T> Forbidden(string message = "Forbidden")
        {
            return new BaseResponse<T>(HttpStatusCode.Forbidden, false, message, default);
        }

        public static BaseResponse<T> FailWithData(string message, T data, HttpStatusCode code)
        {
            return new BaseResponse<T>(code, false, message, data);
        }

        #endregion
    }
}
