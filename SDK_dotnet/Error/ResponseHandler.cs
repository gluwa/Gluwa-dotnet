using SDK_dotnet.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace SDK_dotnet.Error
{
    public static class ResponseHandler
    {
        public const string HTTP_REQUEST_EXCEPTION_CODE = "HttpRequestException";
        public const string HTTP_REQUEST_EXCEPTION_MESSAGE = "An exception occured. Please check your internet connection and try again.";

        public static async Task<Result<T, ErrorResponse>> GetResultAsync<T>(HttpResponseMessage response, string url, Result<T, ErrorResponse> result) where T : class
        {
            string contentString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                result.Data = Converter.FromJson<T>(contentString);
                result.IsSuccess = result.Data != null;

                if (result.IsFailure)
                {
                    result.Error = new ErrorResponse()
                    {
                        Code = "DeserializationError",
                        Message = $"Failed to deserialize {contentString} from {url}"
                    };
                }

                return result;
            }

            result.Error = GetError(response.StatusCode, url, contentString);

            return result;
        }

        public static ErrorResponse GetError(System.Net.HttpStatusCode status, string url, string contentString)
        {
            ErrorResponse error = Converter.FromJson<ErrorResponse>(contentString);

            if (error == null)
            {
                error = new ErrorResponse()
                {
                    Code = "DeserializationError",
                    Message = $"Failed to deserialize {contentString} from {url}"
                };
            }

            return error;
        }

        public static ErrorResponse GetExceptionError()
        {
            return new ErrorResponse()
            {
                Code = HTTP_REQUEST_EXCEPTION_CODE,
                Message = HTTP_REQUEST_EXCEPTION_MESSAGE
            };
        }
    }
}
