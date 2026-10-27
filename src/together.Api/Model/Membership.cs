namespace together.Api.Model
{
    public class Membership
    {
        public int UserId { get; set; }
        public AppUser? User { get; set; }
        public int GroupId { get; set; }
        public SportGroup? Group { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
