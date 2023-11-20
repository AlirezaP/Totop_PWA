using BlazorApp2.Client.Models.Totp;
using BlazorApp2.Client.Security;
using IndexedDB.Blazor;
using Microsoft.JSInterop;
using static BlazorApp2.Client.Models.Totp.TotpDBContext;

namespace BlazorApp2.Client.Services
{
    public class DataBase : IDataBase
    {
        private IAes _aes;
        private IJSRuntime _ijs;
        private IIndexedDbFactory _dbFactory;
        public DataBase(IIndexedDbFactory DbFactory, IJSRuntime ijs, string keyPass)
        {
            string keyStr = Helper.SecurityHelper.GetKeyStrFromPassword(keyPass);

            _ijs = ijs;
            _aes = new Security.Aes(_ijs, keyStr);
            _dbFactory = DbFactory;
        }
        public async Task<TotpInfo[]> GetAllTotp()
        {
            using (var db = await _dbFactory.Create<TotpDBContext>(version:3))
            {
                string password = "secret";

                var all = db.DataSec.ToList();

                if (all is null || all.Count <= 0)
                {
                    return null;
                }

                string keyStr = Helper.SecurityHelper.GetKeyStrFromPassword(password);

                List<TotpInfo> res = new List<TotpInfo>();

                foreach (var totp in all)
                {
                    try
                    {
                        var tmp = await _aes.Decrypt(totp.SecData);
                        var model = Newtonsoft.Json.JsonConvert.DeserializeObject<TotpInfo>(tmp);
                        res.Add(model);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return res.ToArray(); ;

            }
        }

        public async void SaveTotp(TotpInfo model)
        {
            if (model is null)
            {
                throw new Exception("Null Value!");
            }

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            string encrypted = await _aes.Encrypt(jsonData);

            var modelSec = new TotpInfoSec
            {
                SecData = encrypted
            };

            using (var db = await _dbFactory.Create<TotpDBContext>())
            {
                db.DataSec.Add(modelSec);
                await db.SaveChanges();
            }
        }
    }
}
