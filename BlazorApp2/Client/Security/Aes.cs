using Blazor.SubtleCrypto;
using Microsoft.JSInterop;

namespace BlazorApp2.Client.Security
{
    public class Aes : IAes
    {
        private string _key;
        private CryptoService _cryptoService;
        public Aes(IJSRuntime ijs, string key)
        {
            _key = key;
            _cryptoService =
                new CryptoService(ijs, new CryptoOptions
                { Algorithm = AlgorithmType.Sha256, Encryption = EncryptionType.AES_GCM, Key = key });

        }

        public async Task<string> Encrypt(string data)
        {
            CryptoResult encrypted = await _cryptoService.EncryptAsync(data);
            return encrypted.Value;
        }

        public async Task<string> Decrypt(string data)
        {
            var encrypted = await _cryptoService.DecryptAsync(data);
            return encrypted;
        }
    }
}
