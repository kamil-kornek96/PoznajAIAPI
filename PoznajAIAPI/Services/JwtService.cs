﻿using Microsoft.IdentityModel.Tokens;
using PoznajAI.Models.Auth;
using PoznajAI.Models.User;
using PoznajAI.Services;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config, IUserService userService)
    {
        _config = config;
        _userService = userService;
        _secretKey = config["JwtSettings:Key"];
        _issuer = config["JwtSettings:Issuer"];
        _audience = config["JwtSettings:Audicence"];
    }

    public TokenResponseDto GenerateToken(UserDto userDto)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Hash, userDto.Id.ToString())
            //new Claim(ClaimTypes.Role, userDto.Role) // Dodaj claim z rolą
        }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponseDto { Token = tokenHandler.WriteToken(token) };
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Could not create token. UserId: {userDto.Id}");
            return new TokenResponseDto { Token = string.Empty };
        }


    }

    public UserDto FastValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
                var id = claimsPrincipal.FindFirst(ClaimTypes.Hash)?.Value;
                var userDto = new UserDto { Email = email, Id = new Guid(id), Username = username };

                return userDto;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Token validation failed. Token: {token}");
            return null;
        }
    }

    public async Task<UserDto> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
                var id = claimsPrincipal.FindFirst(ClaimTypes.Hash)?.Value;
                var userDto = await _userService.GetUserById(new Guid(id));

                return userDto;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Token validation failed. Token: {token}");
            return null;
        }
    }
}
