using BlazorApp2.Client.Models.Totp;

namespace BlazorApp2.Client.Services
{
    public interface IDataBase
    {
        Task<TotpDBContext.TotpInfo[]> GetAllTotp();
        void SaveTotp(TotpDBContext.TotpInfo model);
    }
}