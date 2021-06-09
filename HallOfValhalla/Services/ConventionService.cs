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
                return new Convention {
                    Id = Guid.Parse(conventionDto.Id),
                    Name = conventionDto.Name,
                    Talks = new HashSet<Talk>(
                        conventionDto.Talks.Select(x => new Talk
                        {
                            Id = Guid.Parse(x.Id),
                            Title = x.Title,
                            Description = x.Description,
                            Speaker = x.Speaker
                        })
                    )
                };
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<Convention>> GetConventionsAsync()
        {
            var query = _container.GetItemQueryIterator<ConventionDto>();
            List<Convention> conventions = new();
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
                if (conventionDto.Participants is null)
                {
                    conventionDto.Participants = new HashSet<string> { userId };
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

        public async Task<bool> ReserveTalkAsync(Guid talkId, string userId)
        {
            var query = _container.GetItemQueryIterator<ConventionDto>($"SELECT * FROM c WHERE ARRAY_CONTAINS(c.talks, {{ id: \"{talkId}\" }}, true)");

            if (!query.HasMoreResults)
                return false;

            while (query.HasMoreResults)
            {
                foreach (var conventionDto in await query.ReadNextAsync())
                {
                    TalkDto talkDto = conventionDto.Talks.First<TalkDto>(t => t.Id == talkId.ToString());

                    if (talkDto.Participants is null)
                    {
                        talkDto.Participants = new HashSet<string> { userId };
                    }
                    else
                    {
                        talkDto.Participants.Add(userId);
                    }

                    await _container.UpsertItemAsync<ConventionDto>(conventionDto);
                }
            }

            return true;
        }

        public async Task<bool> CreateTalkAsync(Guid conventionId, Talk talk)
        {
            TalkDto talkDto = new()
            {
                Id = Guid.NewGuid().ToString(),
                Title = talk.Title,
                Description = talk.Description,
                Speaker = talk.Speaker
            };

            try
            {
                ItemResponse<ConventionDto> response = await _container.ReadItemAsync<ConventionDto>(conventionId.ToString(), new PartitionKey(conventionId.ToString()));
                var conventionDto = response.Resource;
                if (conventionDto.Talks is null)
                {
                    conventionDto.Talks = new HashSet<TalkDto> { talkDto };
                }
                else
                {
                    conventionDto.Talks.Add(talkDto);
                }

                await _container.UpsertItemAsync<ConventionDto>(conventionDto, new PartitionKey(conventionDto.Id));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<bool> IsParticipantRegisteredToConventionAsync(Guid conventionId, string userId)
        {
            string queryString = $"SELECT 1 FROM c WHERE EXISTS(SELECT VALUE p FROM p IN c.participants WHERE c.id = \"{conventionId}\" AND p = \"{userId}\")";
            var query = _container.GetItemQueryIterator<ConventionDto>(queryString);

            while (query.HasMoreResults)
            {
                foreach (var _ in await query.ReadNextAsync())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
