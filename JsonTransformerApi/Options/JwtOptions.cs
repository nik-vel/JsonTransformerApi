namespace JsonTransformerApi.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public HashSet<string> AllowedPasswords { get; set; } = new();
        public int TtlMinutes { get; set; }
    }
}
