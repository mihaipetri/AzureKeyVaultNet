namespace AzureKeyVaultNet.Models
{
    public class SecretModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string Url { get; set; }

        public string Version { get; set; }
    }
}