namespace Contracts.Requests
{
    public class UserRegiterationRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}