namespace E_Commerce.API.Helpers
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}
