namespace FitnessAppAPI.DTOs.User
{
    public class TrainerFullInfoGetDto
    {
        public TrainerGetDto Trainer { get; set; }
        public List<ReviewGetDto> Reviews { get; set; }
    }
}
