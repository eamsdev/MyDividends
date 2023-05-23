using GraphQL.Query.Builder;
using GraphQL.Query.Builder.Formatter.SystemTextJson;

namespace Infrastructure.Providers.Morningstar.V1;

public static class QueryBuilder
{
    private const string QueryName = "getUpcomingDividend";
    
    public static string BuildUpComingDividendQuery(Arguments arguments)
    {
        var options = new QueryOptions
        {
            Formatter = SystemTextJsonPropertyNameFormatter.Format
        };
        
        return new Query<Api.GetUpComingDividendResponse>(QueryName, options)
            .AddField(x => x.PaginatedSecurities, 
                query => query
                    .AddQueryArguments(arguments)
                    .AddPaginatedFields()
                    .AddField(y => y.Securities,  
                        innerQuery => innerQuery
                            .AddSecurityFields())).Build();
    }

    private static IQuery<Api.Security> AddSecurityFields(
        this IQuery<Api.Security> query)
    {
        return query.AddField(x => x.Ticker)
            .AddField(x => x.Name)
            .AddField(x => x.SecurityType)
            .AddField(x => x.ExchangeCode)
            .AddField(x => x.FrankedRate)
            .AddField(x => x.ExDate)
            .AddField(x => x.PayDate)
            .AddField(x => x.DivCashAmount)
            .AddField(x => x.Currency);
    }

    private static IQuery<Api.PaginatedSecurities> AddPaginatedFields(
        this IQuery<Api.PaginatedSecurities> query)
    {
        return query
            .AddField(y => y.Page)
            .AddField(y => y.Total)
            .AddField(y => y.PageSize);
    }
    
    private static IQuery<Api.PaginatedSecurities> AddQueryArguments(
        this IQuery<Api.PaginatedSecurities> query, 
        Arguments arguments)
    {
        return query.AddArguments(new
        {
            universeIds = arguments.UniverseIds,
            filters = arguments.DateFilters,
            sortOrder = arguments.SortOrder,
            page = arguments.Page,
            pageSize = arguments.PageSize
        });
    }

    private static string BuildPayDateFilterBetween(DateTime startDate, TimeSpan timeFromStartDate)
        => $"PayDate:BTW:{startDate:yyyy-MM-dd}:{startDate.Add(timeFromStartDate):yyyy-MM-dd}";
    
    public class Arguments
    {
        public Arguments(
            DateTime startDate, 
            TimeSpan timeFromStartDate, 
            string universeId, 
            int page, 
            int pageSize, 
            string sortOrder = "PayDate asc")
        {
            DateFilters = new[] { BuildPayDateFilterBetween(startDate, timeFromStartDate) };
            UniverseIds = new[] { universeId };
            SortOrder = sortOrder;
            PageSize = pageSize;
            Page = page;
        }

        public string[] DateFilters { get; set; }
        public string[] UniverseIds { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortOrder { get; set; }
    }
}