using AzureKeyVaultNet.Models;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Configuration;
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
                //azureServiceTokenProvider = new AzureServiceTokenProvider();
                //keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
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
        public ActionResult Encryption(EncryptModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CipherText = "CipherText";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"{errorMessage} {ex.Message}";
            }

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
        public ActionResult Decryption(EncryptModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.PlainText = "PlainText";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"{errorMessage} {ex.Message}";
            }

            return View(model);
        }
    }
}
