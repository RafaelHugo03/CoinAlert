using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Interfaces;
using CoinAlertApi.Domain.Entities;
using CoinAlertApi.Domain.Enums;
using CoinAlertApi.Domain.Interfaces;

namespace CoinAlertApi.Application.Services;

public class OpportunityService(
    IOpportunityRepository repository,
    ICacheService cacheService,
    ILogger<OpportunityService> logger) : IOpportunityService
{
    public async Task<List<OpportunityDto>> GetAllAsync()
    {
        var cached = await cacheService.GetAsync<List<OpportunityDto>>(CacheKeys.Opportunities);
        if (cached is not null)
        {
            logger.LogInformation("Opportunities served from cache");
            return cached;
        }

        var entities = await repository.GetAllAsync();
        var dtos = entities.Select(OpportunityDto.FromEntity).ToList();

        await cacheService.SetAsync(CacheKeys.Opportunities, dtos);
        return dtos;
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
        await cacheService.InvalidateAsync(CacheKeys.Opportunities);

        logger.LogInformation("Opportunity created — Id: {Id}", entity.Id);
        return OpportunityDto.FromEntity(entity);
    }

    public async Task<bool> DeleteAsync(string id)
    {

        var deleted = await repository.DeleteAsync(id);

        if (!deleted)
        {
            logger.LogWarning("Attempted to delete non-existent opportunity — Id: {Id}", id);
            return false;
        }

        await cacheService.InvalidateAsync(CacheKeys.Opportunities);
        logger.LogInformation("Opportunity deleted — Id: {Id}", id);
        return true;
    }
}
