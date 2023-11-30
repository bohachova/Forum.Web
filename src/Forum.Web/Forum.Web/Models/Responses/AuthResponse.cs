namespace Forum.Web.Models.Responses
{
    public class AuthResponse : Response
    {
        public string JWTToken { get; set; } = string.Empty;
    }
}
