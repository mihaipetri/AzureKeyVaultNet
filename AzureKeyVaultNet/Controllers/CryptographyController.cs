using AzureKeyVaultNet.Models;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AzureKeyVaultNet.Controllers
{
    public class CryptographyController : Controller
    {
        private string azureKeyVaultUrl = ConfigurationManager.AppSettings["AzureKeyVaultUrl"];
        private AzureServiceTokenProvider azureServiceTokenProvider;
        private KeyVaultClient keyVaultClient;
        private string errorMessage = "Error:";

        public CryptographyController()
        {
            try
            {
                azureServiceTokenProvider = new AzureServiceTokenProvider();
                keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            }
            catch (Exception ex)
            {
                errorMessage = $"{errorMessage} {ex.Message}";
            }
        }

        [HttpGet]
        public ActionResult Encryption()
        {
            EncryptModel model = new EncryptModel
            {
                Key = "Please enter a key",
                PlainText = "Please enter a plain text"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Encryption(EncryptModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    KeyOperationResult operationResult = await keyVaultClient.EncryptAsync(azureKeyVaultUrl + "/keys/" + model.Key, Microsoft.Azure.KeyVault.WebKey.JsonWebKeyEncryptionAlgorithm.RSAOAEP, System.Text.Encoding.UTF8.GetBytes(model.PlainText)).ConfigureAwait(false);
                    model.CipherText = Convert.ToBase64String(operationResult.Result);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"{errorMessage} {ex.Message}";
            }

            ModelState.Clear();
            return View(model);
        }

        [HttpGet]
        public ActionResult Decryption()
        {
            EncryptModel model = new EncryptModel
            {
                Key = "Please enter a key",
                CipherText = "Please enter a cipher text"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Decryption(EncryptModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    KeyOperationResult operationResult = await keyVaultClient.DecryptAsync(azureKeyVaultUrl + "/keys/" + model.Key, Microsoft.Azure.KeyVault.WebKey.JsonWebKeyEncryptionAlgorithm.RSAOAEP, Convert.FromBase64String(model.CipherText)).ConfigureAwait(false);
                    model.PlainText = System.Text.Encoding.UTF8.GetString(operationResult.Result);                   
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"{errorMessage} {ex.Message}";
            }

            ModelState.Clear();
            return View(model);
        }
    }
}
