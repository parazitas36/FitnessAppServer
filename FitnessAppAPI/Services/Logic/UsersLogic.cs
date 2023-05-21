using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Client;
using FitnessAppAPI.DTOs.User;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Services.Logic;

public class UsersLogic : IUsersLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public UsersLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserGetDto> Login(string username, string password)
    {
        User? user = _dbContext.Users.Include(x => x.ContactInfo).FirstOrDefault(x => x.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        return new UserGetDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role.ToString(),
            ContactInfo = new ContactInfoDto
            {
                Email = user.ContactInfo?.EmailAddress,
                PhoneNumber = user.ContactInfo?.PhoneNumber,
            },
            Name = user.Name,
            Surname = user.Surname,
            IsPublicName = user.IsPublicName,
            UsesImperialSystem = user.UsesImperialSystem,
        };
    }

    public async Task<bool> Register(UserRegisterDto user)
    {
        if (user.Password != user.RepeatPassword)
        {
            return false;
        }

        User? existingUser = _dbContext.Users.FirstOrDefault(x => x.Username == user.Username || x.Email == user.Email);

        if (existingUser != null)
        {
            return false;
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

        ContactInfo? contactInfo = null;

        try
        {
            if (user.ContactInfo != null)
            {
                contactInfo = (await _dbContext.ContactInfo.AddAsync(new ContactInfo
                {
                    EmailAddress = user.ContactInfo?.Email,
                    PhoneNumber = user.ContactInfo?.PhoneNumber,
                })).Entity;
            }


            await _dbContext.Users.AddAsync(new User
            {
                Username = user.Username,
                Password = passwordHash,
                ContactInfo = contactInfo,
                Email = user.Email,
                IsPublicName = user.IsPublicName,
                Name = user.Name,
                Surname = user.Surname,
                Role = Enum.TryParse<Roles>(user.Role.ToString(), out Roles role) ? role : Roles.User,
                UsesImperialSystem = user.UsesImperialSystem
            });

            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<List<TrainerGetDto>> GetTrainers(int? sportsClubAdminId = null)
    {
        var trainers = await _dbContext.Users.Where(x => x.Role == Roles.Trainer)
            .Select(x => new TrainerGetDto
            {
                Id = x.Id,
                Name = x.Name,
                LastName = x.Surname,
                Username = x.Username,
                Email = _dbContext.ContactInfo.FirstOrDefault(y => x.ContactInfo != null && y.Id == x.ContactInfo.Id).EmailAddress,
                Phone = _dbContext.ContactInfo.FirstOrDefault(y => x.ContactInfo != null && y.Id == x.ContactInfo.Id).PhoneNumber,
                AverageRating = _dbContext.Reviews.Where(y => y.Trainer.Id == x.Id).Average(y => y.Rating),
                ReviewsCount = _dbContext.Reviews.Count(y => y.Trainer.Id == x.Id),
                IsInvited = sportsClubAdminId != null ? _dbContext.TrainerInvites.Any(x => x.InvitedBy.Id == sportsClubAdminId) : null,
            }).ToListAsync();

        return Task.FromResult(trainers).Result;
    }

    public async Task<TrainerFullInfoGetDto?> GetTrainerInfo(int trainerId, int? sportsClubAdminId = null)
    {
        var trainer = await _dbContext.Users.Where(x => x.Id == trainerId)
            .Select(x => new TrainerFullInfoGetDto
            {
                Trainer = new TrainerGetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.Surname,
                    Username = x.Username,
                    Email = _dbContext.ContactInfo.FirstOrDefault(y => x.ContactInfo != null && y.Id == x.ContactInfo.Id).EmailAddress,
                    Phone = _dbContext.ContactInfo.FirstOrDefault(y => x.ContactInfo != null && y.Id == x.ContactInfo.Id).PhoneNumber,
                    AverageRating = _dbContext.Reviews.Where(y => y.Trainer.Id == x.Id).Average(y => y.Rating),
                    ReviewsCount = _dbContext.Reviews.Count(y => y.Trainer.Id == x.Id),
                    IsInvited = sportsClubAdminId != null ? _dbContext.TrainerInvites.Any(y => y.InvitedBy.Id == sportsClubAdminId && y.Trainer.Id == x.Id) : null,
                },
                Reviews = _dbContext.Reviews.Include(y => y.CreatedBy).Where(y => y.Trainer.Id == trainerId)
                .Select(r => new ReviewGetDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Review = r.ReviewText,
                    User = new UserShortGetDto
                    {
                        Id = r.CreatedBy.Id,
                        Username = r.CreatedBy.Username
                    }
                }).ToList()
            }).FirstOrDefaultAsync();

        return Task.FromResult(trainer).Result;
    }

    public async Task<bool> PostReview(int userId, ReviewPostDto dto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        SportsClub? sportsClub = null;
        User? trainer = null;

        if (user == null) { return Task.FromResult(false).Result; }

        if (dto.ReviewedSportsClubId != null)
        {
            sportsClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Id == dto.ReviewedSportsClubId);
        }
        else if (dto.ReviewedTrainerId != null)
        {
            trainer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == dto.ReviewedTrainerId);
        }

        if (trainer == null && sportsClub == null) { return Task.FromResult(false).Result; }

        try
        {
            var review = new Review
            {
                CreatedDate = DateTime.Now,
                CreatedBy = user,
                Rating = dto.Rating,
                ReviewText = dto.ReviewText,
                SportsClub = sportsClub,
                Trainer = trainer,
            };

            var existing = await _dbContext.Reviews.Where(x =>
                x.CreatedBy.Id == userId &&
                ((trainer != null && trainer.Id == x.Trainer.Id) || (sportsClub != null && sportsClub.Id == x.SportsClub.Id)))
                .FirstOrDefaultAsync();
                

            if (existing != null)
            {
                existing.CreatedDate = review.CreatedDate;
                existing.Rating = review.Rating;
                existing.ReviewText = review.ReviewText;

                _dbContext.Reviews.Update(existing);
                await _dbContext.SaveChangesAsync();

                return Task.FromResult(true).Result;
            }

            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<List<ClientGetDto>> GetTrainerClients(int trainerId)
    {
        var result = (await _dbContext.ClientTrainingPlans
            .Include(x => x.Client)
            .Where(x => x.TrainingPlan.CreatedBy.Id == trainerId)
            .Select(x => new ClientGetDto
            {
                Id = x.Client.Id,
                Username = x.Client.Username,
                Name = x.Client.Name,
                Surname = x.Client.Surname,
                Email = _dbContext.ContactInfo.FirstOrDefault(y => y.Id == x.Client.ContactInfo.Id).EmailAddress,
                PhoneNumber = _dbContext.ContactInfo.FirstOrDefault(y => y.Id == x.Client.ContactInfo.Id).PhoneNumber,
                TrainingPlansAssigned = _dbContext.ClientTrainingPlans.Count(y => y.Client.Id == x.Client.Id && y.TrainingPlan.CreatedBy.Id == x.TrainingPlan.CreatedBy.Id)
            }).ToListAsync()).DistinctBy(x => x.Id).ToList();

        return Task.FromResult(result).Result;
    }

    public async Task<List<UserGetDto>> FindUsersByUsername(string username)
    {
        var users = await _dbContext.Users.Where(x => x.Role == Roles.User && x.Username.Contains(username))
            .Select(x => new UserGetDto
            {
                Id = x.Id,
                Username = x.Username,
                Name = x.Name,
                Surname = x.Surname,
                ContactInfo = new ContactInfoDto
                {
                    Email = _dbContext.ContactInfo.FirstOrDefault(y => y.Id == x.ContactInfo.Id).EmailAddress,
                    PhoneNumber = _dbContext.ContactInfo.FirstOrDefault(y => y.Id == x.ContactInfo.Id).PhoneNumber,
                }
            }).ToListAsync();

        return Task.FromResult(users).Result;
    }
}
