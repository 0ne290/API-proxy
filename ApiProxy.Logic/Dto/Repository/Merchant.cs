using System.Security.Cryptography;
using System.Text;

namespace ApiProxy.Logic.Dto.Repository;

public class Merchant
{
    public Merchant(string? guid, string? date, string? login, string? password, string? redirectUrl, string? callbackUrl)
    {
        Guid = guid;
        Date = date;
        Login = login;
        Password = password;
        RedirectUrl = redirectUrl;
        CallbackUrl = callbackUrl;
    }

    public Merchant()
    {
        Guid = $"{System.Guid.NewGuid()}";
        Date = $"{DateTime.UtcNow}";
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData)).Select(t => $"{t:x2}");
        return string.Join("", bytes);
    }

    private string? _login, _password;

    public string? Guid { get; }
    public string? Date { get; }
    public string? Login
    {
        get => _login;
        set => _login = ComputeSha256Hash(value + Guid + Date);
    }
    public string? Password
    {
        get => _password;
        set => _password = ComputeSha256Hash(value + Guid + Date);
    }
    public string? RedirectUrl { get; set; }
    public string? CallbackUrl { get; set; }

}