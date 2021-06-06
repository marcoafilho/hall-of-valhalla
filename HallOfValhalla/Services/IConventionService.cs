using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HallOfValhalla.Domain;

namespace HallOfValhalla.Services
{
    public interface IConventionService
    {
        Task<List<Convention>> GetConventionsAsync();

        Task<Convention> GetConventionByIdAsync(Guid conventionId);

        Task<bool> CreateConventionAsync(Convention convention);

        Task<bool> UpdateConventionAsync(Convention conventionToUpdate);

        Task<bool> DeleteConventionAsync(Guid conventionId);

        Task<bool> AddParticipantAsync(Guid conventionId, string userId);

        Task<bool> ReserveTalkAsync(Guid talkId, string userId);
    }
}
