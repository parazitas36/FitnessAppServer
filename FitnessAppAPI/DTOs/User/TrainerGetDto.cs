namespace FitnessAppAPI.DTOs.User
{
    public class TrainerGetDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Id { get; set; }
        public double? AverageRating { get; set; } = null;
        public int ReviewsCount { get; set; } = 0;
        public string? Email { get; set; } = null;
        public string? Phone { get; set; } = null;
    }
}
