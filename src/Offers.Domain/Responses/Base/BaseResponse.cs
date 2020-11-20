namespace Offers.Domain.Responses.Base
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            IsSuccess = true;
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
