using CarniceriaFinal.Autenticacion.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core
{
    public interface IJwtUtils
    {
        public string? generateJwtToken(UserTokenEntity user);
        public int? ValidateToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        public IConfiguration Configuration { get; }
        public JwtUtils(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string? generateJwtToken(UserTokenEntity user)
        {
            var secret = Configuration.GetValue<string>("AppSettings:Secret");

            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] { 
                        new Claim("idUser", user.idUser.ToString()),
                        new Claim("idRol", user.idRol.ToString()) 
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public int? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "idUser").Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
