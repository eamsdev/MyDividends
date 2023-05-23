using Domain.Entities;
using Ardalis.Specification;

namespace Application.Features.Dividends.Specifications;

public sealed class SecuritiesMatchingGivenSet : Specification<Security>
{
    public SecuritiesMatchingGivenSet(IEnumerable<Security> sampleSet)
    {
        Query.Where(x => sampleSet.Select(y => y.Reference).Contains(x.Reference));
    }
}