using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallOfValhalla.Domain;
using Microsoft.Azure.Cosmos;

namespace HallOfValhalla.Services
{
    public class ConventionService : IConventionService
    {
        private Container _container;

        private List<Convention> _conventions;

        public ConventionService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<bool> CreateConventionAsync(Convention convention)
        {
            var conventionDto = new ConventionDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = convention.Name
            };

            var response = await _container.CreateItemAsync<ConventionDto>(conventionDto, new PartitionKey(conventionDto.Id));
            convention.Id = Guid.Parse(conventionDto.Id);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;

            return false;
        }

        public async Task<bool> DeleteConventionAsync(Guid conventionId)
        {
            try
            {
                await _container.DeleteItemAsync<ConventionDto>(conventionId.ToString(), new PartitionKey(conventionId.ToString()));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<Convention> GetConventionByIdAsync(Guid conventionId)
        {
            try
            {
                ItemResponse<ConventionDto> response = await _container.ReadItemAsync<ConventionDto>(conventionId.ToString(), new PartitionKey(conventionId.ToString()));
                var conventionDto = response.Resource;
                return new Convention { Id = Guid.Parse(conventionDto.Id), Name = conventionDto.Name };
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<Convention>> GetConventionsAsync()
        {
            var query = _container.GetItemQueryIterator<ConventionDto>();
            List<Convention> conventions = new List<Convention>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                conventions.AddRange(response.Select(x => new Convention { Id = Guid.Parse(x.Id), Name = x.Name }).ToList());
            }

            return conventions;
        }

        public async Task<bool> UpdateConventionAsync(Convention conventionToUpdate)
        {
            var conventionDto = new ConventionDto
            {
                Id = conventionToUpdate.Id.ToString(),
                Name = conventionToUpdate.Name
            };

            try
            {
                await _container.UpsertItemAsync<ConventionDto>(conventionDto, new PartitionKey(conventionDto.Id));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<bool> AddParticipantAsync(Guid conventionId, string userId)
        {
            try
            {
                ItemResponse<ConventionDto> response = await _container.ReadItemAsync<ConventionDto>(conventionId.ToString(), new PartitionKey(conventionId.ToString()));
                var conventionDto = response.Resource;
                if (conventionDto.Participants == null)
                {
                    conventionDto.Participants = new List<string>();
                    conventionDto.Participants.Add(userId);
                }
                else
                {
                    conventionDto.Participants.Add(userId);
                }

                await _container.UpsertItemAsync<ConventionDto>(conventionDto, new PartitionKey(conventionDto.Id));

                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
    }
}
