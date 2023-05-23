using Domain.Entities;

namespace Application.Features.Dividends.Interfaces;

public interface ISecuritiesInfoProvider
{
    Task<List<Security>> GetSecuritiesInfo(CancellationToken token);
}