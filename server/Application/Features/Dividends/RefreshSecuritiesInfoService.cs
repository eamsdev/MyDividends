using Application.Features.Dividends.Interfaces;
using Application.Features.Dividends.Specifications;
using Ardalis.Specification;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dividends;

public class RefreshSecuritiesInfoService
{
    private readonly IRepositoryBase<Security> _repository;
    private readonly ISecuritiesInfoProvider _securitiesProvider;
    private readonly ILogger<RefreshSecuritiesInfoService> _logger;

    public RefreshSecuritiesInfoService(
        ISecuritiesInfoProvider securitiesProvider,
        IRepositoryBase<Security> repository,
        ILogger<RefreshSecuritiesInfoService> logger)
    {
        _logger = logger;
        _repository = repository;
        _securitiesProvider = securitiesProvider;
    }

    public async Task Refresh(CancellationToken token = default)
    {
        var distinctSecurities = (await _securitiesProvider.GetSecuritiesInfo(token))
            .DistinctBy(x => x.Reference)
            .ToList();
        
        var securityMatchingSetSpec = new SecuritiesMatchingGivenSet(distinctSecurities);
        var existingEntities = await _repository.ListAsync(securityMatchingSetSpec, token);
        

        foreach (var existingEntity in existingEntities)
        {
            var updatedSecurity = distinctSecurities.Single(x =>
                x.Reference == existingEntity.Reference);
            
            UpdateExistingSecurity(existingEntity, updatedSecurity);
            distinctSecurities.Remove(updatedSecurity);
        }
        
        await _repository.AddRangeAsync(distinctSecurities, token);
        await _repository.SaveChangesAsync(token);
        
        
        _logger.LogInformation("{ExistingSecuritiesCount} existing securities are updated", 
            existingEntities.Count);
        
        _logger.LogInformation("{NewSecuritiesCount} securities are added", 
            distinctSecurities.Count);
    }

    private static void UpdateExistingSecurity(Security existingEntity, Security updatedSecurity)
    {
        existingEntity.Name = updatedSecurity.Name;
        existingEntity.ExDate = updatedSecurity.ExDate;
        existingEntity.FrankedRate = updatedSecurity.FrankedRate;
        existingEntity.ExchangeCode = updatedSecurity.ExchangeCode;
        existingEntity.ExDate = updatedSecurity.ExDate;
        existingEntity.PayDate = updatedSecurity.PayDate;
        existingEntity.DivCashAmount = updatedSecurity.DivCashAmount;
        existingEntity.Currency = updatedSecurity.Currency;
        existingEntity.Reference = updatedSecurity.Reference;
    } 
}