using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Dividend.Specifications;

public sealed class SecurityByTickerAndExchange : SingleResultSpecification<Security>
{
    public SecurityByTickerAndExchange(string ticker, string exchangeCode)
    {
        Query.Where(x => x.Ticker == ticker && x.ExchangeCode == exchangeCode);
    }
}