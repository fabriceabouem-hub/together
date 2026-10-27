namespace together.Api.Model
{
    public class SportGroup
    {   
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string City { get; set; } = string.Empty;
        public string Level { get; set; } = "Alle Niveaus";
        public int SportId { get; set; }
        public Sport? Sport { get; set; }
        public int CreatorId { get; set; }
        public AppUser? Creator { get; set; }
        public List<Membership> Memberships { get; set; } = new();
    }
}
