using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Interfaces;
using CoinAlertApi.Domain.Entities;
using CoinAlertApi.Domain.Enums;
using CoinAlertApi.Domain.Interfaces;

namespace CoinAlertApi.Application.Services;

public class OpportunityService : IOpportunityService
{
    private readonly IOpportunityRepository _repository;
    public OpportunityService(IOpportunityRepository repository)
    {
        _repository = repository;
    }


    public async Task<List<OpportunityDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
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

        await _repository.InsertAsync(entity);

        var result = OpportunityDto.FromEntity(entity);
        return result;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _repository.DeleteAsync(id);
    }
}
