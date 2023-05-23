using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Dividend.Specifications;

public sealed class SecuritiesMatchingGivenSet : Specification<Security>
{
    public SecuritiesMatchingGivenSet(IEnumerable<Security> sampleSet)
    {
        Query.Where(x => sampleSet.Select(y => y.Reference).Contains(x.Reference));
    }
}