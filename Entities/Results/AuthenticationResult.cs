using System.Collections.Generic;

namespace Entities.Results
{
    public class AuthenticationResult
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int ExpiredIn { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}