using ApiCatalogo2.Models;

namespace ApiCatalogo2.Services;

public interface ITokenService
{
    string GerarToken(string key, string issuer, string audience, UserModel user);
}
