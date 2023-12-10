namespace WebApi.Models;

using MeetingPlannerAPI.Model;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(Users user, string token)
    {
        Id = (int)user.uid;
        Username = user.user_email;
        Token = token;
    }
}