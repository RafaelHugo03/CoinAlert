using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Interfaces;
using CoinAlertApi.Domain.Entities;
using CoinAlertApi.Domain.Enums;
using CoinAlertApi.Domain.Interfaces;

namespace CoinAlertApi.Application.Services;

public class OpportunityService(
    IOpportunityRepository repository,
    ILogger<OpportunityService> logger) : IOpportunityService
{
    public async Task<List<OpportunityDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        return entities.Select(OpportunityDto.FromEntity).ToList();
    }

    public async Task<OpportunityDto> CreateAsync(CreateOpportunityDto dto)
    {
        var entity = new Opportunity
        {
            Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            CryptoId = dto.CryptoId,
            Type = dto.Type,
            TargetPrice = dto.TargetPrice,
            Status = OpportunityStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await repository.InsertAsync(entity);

        logger.LogInformation("Opportunity created — Id: {Id}", entity.Id);
        return OpportunityDto.FromEntity(entity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await repository.DeleteAsync(id);

        if(!deleted){
            logger.LogWarning("Attempted to delete non-existent opportunity — Id: {Id}", id);
            return deleted;
        }

        logger.LogInformation("Opportunity deleted — Id: {Id}", id);
        return deleted;
    }
}
