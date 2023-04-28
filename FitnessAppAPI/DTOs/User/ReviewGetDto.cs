namespace FitnessAppAPI.DTOs.User
{
    public class ReviewGetDto
    {
        public int Id { get; set; }
        public UserShortGetDto User { get; set; }
        public int? Rating { get; set; } = null;
        public string? Review { get; set; } = null;
    }
}
