namespace BlazorApp2.Client.Security
{
    public interface IAes
    {
        Task<string> Decrypt(string data);
        Task<string> Encrypt(string data);
    }
}