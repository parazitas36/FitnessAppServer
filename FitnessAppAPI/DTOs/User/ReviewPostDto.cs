namespace FitnessAppAPI.DTOs.User
{
    public class ReviewPostDto
    {
        public int? ReviewedTrainerId { get; set; }
        public int? ReviewedSportsClubId { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
    }
}
