namespace FitnessAppAPI.DTOs.Forms
{
    public class JobOfferGetDto
    {
        public int Id { get; set; }
        public int TrainerJobFormId { get; set; }
        public int? TrainerId { get; set; }
        public string? TrainerUserName { get; set; }
        public int? SportsClubId { get; set; }
        public string? SportsClubName { get;set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public DateTime OfferDate { get; set; }
    }
}
