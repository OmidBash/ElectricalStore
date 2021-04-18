namespace Entities.Options
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Secret { get; set; }

        public string ExpiredIn { get; set; }
    }
}