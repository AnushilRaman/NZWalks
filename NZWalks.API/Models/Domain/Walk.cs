namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DiffcultyId { get; set; }
        public Guid RegionId { get; set; }
        public Difficulty diffculty { get; set; }
        public Region region { get; set; }

    }
}
