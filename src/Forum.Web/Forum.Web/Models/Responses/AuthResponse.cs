namespace Forum.Web.Models.Responses
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string JWTToken { get; set; } = string.Empty;
    }
}
