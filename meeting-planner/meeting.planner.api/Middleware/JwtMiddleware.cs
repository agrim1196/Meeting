namespace WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUsersService userService)
    {
        try
        {
            var model = new TokenModel();
            // first time true
            var signInRequest = context.Request.Headers["Is-Sign-In"].FirstOrDefault();

            // first time token will we null
            string? tokenString = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (tokenString is not null)
            {
                var startTokenIndex = tokenString.IndexOf('{');
                var endTokenIndex = tokenString.LastIndexOf('}');
                var tokenData = tokenString.Substring(startTokenIndex, endTokenIndex - startTokenIndex + 1);
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenData);
                int id = 0;
                int.TryParse(dictionary?["id"], out id);
                string? username = dictionary?["username"];
                string? token = dictionary?["token"];
                model = new TokenModel
                {
                    Id = id,
                    Username = username,
                    Token = token
                };
                attachUserToContext(model.Token);
            }

            await _next(context);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private IActionResult attachUserToContext(string? token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var result = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return new OkObjectResult(jwtToken);
        }
        catch (SecurityTokenValidationException)
        {
            return new BadRequestResult();
        }
    }
}