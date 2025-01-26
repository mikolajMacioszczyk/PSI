namespace Api.Extensions
{
    public class KeycloakServiceConfig
    {
        public required string Realm { get; set; }
        public required string AuthServerUrl { get; set; }
        public required string ClientId { get; set; }
        public required string Secret { get; set; }
    }
}
