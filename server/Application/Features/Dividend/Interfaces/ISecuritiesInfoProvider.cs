using Domain.Entities;

namespace Application.Features.Dividend.Interfaces;

public interface ISecuritiesInfoProvider
{
    Task<List<Security>> GetSecuritiesInfo(CancellationToken token);
}