namespace MeetingPlannerAPI.Model
{
    public record Users
    {
        public decimal uid { get; set; }

        public string user_email { get; set; }

        public string user_password { get; set; }
    }
}
