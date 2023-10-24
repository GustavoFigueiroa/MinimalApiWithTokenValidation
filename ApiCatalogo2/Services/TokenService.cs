using ApiCatalogo2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCatalogo2.Services
{
    public class TokenService : ITokenService
    {
        public string GerarToken(string key, string issuer, string audience, UserModel user)
        {
            //São declarações do usuario que vão compor o payload do Token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            //Gera uma chave utilizando a chave secreta "key" vindo do parametro
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Aplicando o algoritmo para obter a chave simetrica
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Definir a geração do Token
            var token = new JwtSecurityToken(issuer: issuer,
                                             audience: audience,
                                             claims: claims,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            //desserializar o Token e reotrnar uma string para o ussuario que será o token
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;

        }
    }
}
