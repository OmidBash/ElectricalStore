namespace Contracts.Responses
{
    public class AuthSuccessResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int ExpiredIn { get; set; }
    }
}