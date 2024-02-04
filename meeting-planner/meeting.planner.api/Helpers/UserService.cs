namespace WebApi.Services;

using MeetingPlannerAPI.DAL;
using MeetingPlannerAPI.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Helpers;
using WebApi.Models;

public interface IUsersService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
}

public class UsersService : IUsersService
{
    private readonly AppSettings _appSettings;
    private readonly IRepository<Users> _usersRepo;
    public UsersService(IOptions<AppSettings> appSettings, IRepository<Users> usersRepo)
    {
        _appSettings = appSettings.Value;
        _usersRepo = usersRepo;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        // Convert PASSWORD to hash code SHA-256
        var hash = sha256_hash(model.Password);


        var Users = (await _usersRepo.getAllAsync()).FirstOrDefault();
        //var Users = _usersRepo.getAll();
        // return null if Users not found
        if (Users == null) return null;

        // authentication successful so generate jwt token
        var token = generateJwtToken(Users);

        return new AuthenticateResponse(Users, token);
    }

    // helper methods

    private string generateJwtToken(Users Users)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("email", Users.user_email.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static string sha256_hash(string value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
}