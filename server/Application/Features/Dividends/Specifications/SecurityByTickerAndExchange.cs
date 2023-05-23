using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Dividends.Specifications;

public sealed class SecurityByTickerAndExchange : SingleResultSpecification<Security>
{
    public SecurityByTickerAndExchange(string ticker, string exchangeCode)
    {
        Query.Where(x => x.Ticker == ticker && x.ExchangeCode == exchangeCode);
    }
}