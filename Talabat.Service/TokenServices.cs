﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenServices : ITokenService
    {
        public TokenServices(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            //private claims
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email ,user.Email),
                new Claim(ClaimTypes.GivenName ,user.DisplayName)
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            //Secrete key
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: Configuration["jwt:ValidIssuer"],
                audience: Configuration["jwt:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(Configuration["jwt:DurationInDays"])),
                claims:authClaims,
                signingCredentials:new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)


                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
