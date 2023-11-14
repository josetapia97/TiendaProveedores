namespace API.FurnitoreStore.API.Configuration
{
    public class JwtConfig
    {
        public string Secret {  get; set; }
        public TimeSpan ExpiryTime { get; set; }
    }
}   
