
using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;

namespace WebApi.Utility
{
    public class TokenManager
    {
        public static Dictionary<string, string> GeneratedTokens = new Dictionary<string, string>();
        //static string keyText = "Reuters2020-AASA-BMC";
        //static string encryptionSecureKey = "-AASA-BMCReuters2020";
        private static string Secret = "J8c20cxkPZCC/0e0ZUcjrGocsk95gOAqjzJ09apAklM=";
        private static double TokenExpireTime = 3600.0;

        public static string GenerateToken(Register obj)
        {
            var key = Convert.FromBase64String(Secret);
            var securityKey = new SymmetricSecurityKey(key);
            var claims = new List<Claim>
            {
                new Claim("UserId", Convert.ToString(obj.UserId)),
                new Claim("Username", Convert.ToString(obj.Username)),
                new Claim("Email", Convert.ToString(obj.Email)),
                new Claim("Role", Convert.ToString(obj.RoleIds ?? ""))
            };

            // Add claims for each permission separately

                    if (obj.RoleScreenPermission != null && obj.RoleScreenPermission.Count > 0)
                    {
                        foreach (var permission in obj.RoleScreenPermission ?? Enumerable.Empty<RoleScreenPermission>())
                        {
                            claims.Add(new Claim($"RoleIds:{permission.RoleIds}", string.Empty)); // Create a claim for RoleId
                            claims.Add(new Claim($"ScreenId:{permission.RoleIds}", permission.ScreenId.ToString())); // Create a claim for ScreenId
                            claims.Add(new Claim($"ScreenName:{permission.RoleIds}", permission.ScreenName ?? string.Empty)); // Create a claim for ScreenName
                        }
                    }

                    var descriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Issuer = "CatPedigree",
                        Expires = DateTime.Now.AddMinutes(TokenExpireTime),
                        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.CreateJwtSecurityToken(descriptor);
                    var tokenString = handler.WriteToken(token);

                    return tokenString;
        }


        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Register ValidateToken(string token)
        {
            var principal = GetPrincipal(token);
            if (principal == null) return null;

            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null) return null;

            var obj = new Register
            {
                UserId = int.TryParse(identity.FindFirst("UserId")?.Value, out var userId) ? userId : 0,
                Username = identity.FindFirst("Username")?.Value,
                Email = identity.FindFirst("Email")?.Value,
                RoleIds = identity.FindFirst("Role")?.Value,

                RoleScreenPermission = identity.Claims
                    .Where(claim => claim.Type.StartsWith("RoleIds:"))
                    .GroupBy(claim => claim.Type.Split(':')[1]) // Group by RoleId
                    .Select(group =>
                    {
                        var roleIds = group.Key?.ToString() ?? "";
                        var screenId = int.TryParse(identity.FindFirst($"ScreenId:{roleIds}")?.Value, out var screenIdValue) ? screenIdValue : 0;
                        var screenName = identity.FindFirst($"ScreenName:{roleIds}")?.Value;

                        return new RoleScreenPermission
                        {
                            RoleIds = roleIds,
                            ScreenId = screenId,
                            ScreenName = screenName
                        };
                    })
                    .ToList()
            };

            return obj;
        }

        public static Register GetValidateToken(HttpRequest httpRequest)
        {
            if (!httpRequest.Headers.ContainsKey("Authorization")) return null;

            var authHeader = httpRequest.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader)) return null;

            var token = authHeader;
            var item = GeneratedTokens.FirstOrDefault(kvp => kvp.Value == token);
            if (!item.Equals(default(KeyValuePair<string, string>)))
            {
                GeneratedTokens.Remove(item.Key);
            }

            return ValidateToken(token);
        }
        public static string RemoveToken(string token)
        {
            var item = GeneratedTokens.FirstOrDefault(kvp => kvp.Value == token);
            if (!item.Equals(default(KeyValuePair<string, string>)))
            {
                GeneratedTokens.Remove(item.Key);
                return null;
            }
            else
            {
                return null;
            }

        }

    }
}
