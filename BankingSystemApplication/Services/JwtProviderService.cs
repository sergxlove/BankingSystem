﻿using BankingSystemApplication.Abstractions;
using BankingSystemApplication.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankingSystemApplication.Services
{
    public class JwtProviderService : IJwtProviderService
    {
        public string? GenerateToken(JwtRequest request)
        {
            JwtSecurityToken jwt = new(
                    issuer: request.Issuer,
                    audience: request.Audience,
                    claims: request.Claims,
                    expires: request.Expires,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(request.SecretKey)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
