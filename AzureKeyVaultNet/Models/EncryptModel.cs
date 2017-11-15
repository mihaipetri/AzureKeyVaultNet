namespace AzureKeyVaultNet.Models
{
    public class EncryptModel
    {
        public string Key { get; set; }

        public string PlainText { get; set; }

        public string CipherText { get; set; }
    }
}