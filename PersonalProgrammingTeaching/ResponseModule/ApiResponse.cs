
using System.Net.Http.Headers;

namespace PersonalLearning.ResponseModule
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode,string message=null)
        {
            StatusCode = statusCode;
            Message = message??GetDefaultMessageStatusCode(statusCode);
        }
        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request, You Are Make someting wrong please try again",
                401 => "UnAuthorized, please login or signup if you don't have an account",
                402 => "You Should first complete the payment process",
                500 => "An Problem with the server we try to solve it, please try again later"
            };
        }
    }

}
