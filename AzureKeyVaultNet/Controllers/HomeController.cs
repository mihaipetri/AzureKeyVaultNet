using System;
using System.Web.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;
using System.Configuration;
using AzureKeyVaultNet.Models;
using System.Collections.Generic;

namespace AzureKeyVaultNet.Controllers
{
    public class HomeController : Controller
    {
        private string azureKeyVaultUrl = ConfigurationManager.AppSettings["AzureKeyVaultUrl"];
        private AzureServiceTokenProvider azureServiceTokenProvider;
        private KeyVaultClient keyVaultClient;
        private string errorMessage = "Error:";

        public HomeController()
        {
            try
            {
                azureServiceTokenProvider = new AzureServiceTokenProvider();
                keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            }
            catch(Exception ex)
            {
                errorMessage = $"{errorMessage} {ex.Message}";
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Secrets()
        {
            try
            {
                var keyVaultSecrets = await keyVaultClient.GetSecretsAsync(azureKeyVaultUrl).ConfigureAwait(false);
                IList<SecretModel> secretModelList = new List<SecretModel>();
                foreach(var secret in keyVaultSecrets)
                {
                    var secretModel = new SecretModel
                    {
                        Name = secret.Identifier.Name,
                        Type = secret.ContentType,
                        Url = secret.Id,
                        Version = secret.Identifier.Version
                    };

                    var keyVaultSecretValue = await keyVaultClient.GetSecretAsync(secret.Identifier.Identifier).ConfigureAwait(false);
                    secretModel.Value = keyVaultSecretValue.Value;
                    secretModel.Version = keyVaultSecretValue.SecretIdentifier.Version;
                    secretModelList.Add(secretModel);
                }

                return View(secretModelList);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"{errorMessage} {ex.Message}";
            }

            return View();
        }

        public async Task<ActionResult> Keys()
        {
            try
            {
                var keys = await keyVaultClient.GetKeysAsync(azureKeyVaultUrl).ConfigureAwait(false);
                return View(keys);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"{errorMessage} {ex.Message}";
            }

            return View();
        }

        public async Task<ActionResult> Certificates()
        {
            try
            {
                var certificates = await keyVaultClient.GetCertificatesAsync(azureKeyVaultUrl).ConfigureAwait(false);
                return View(certificates);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"{errorMessage} {ex.Message}";
            }

            return View();
        }
    }
}