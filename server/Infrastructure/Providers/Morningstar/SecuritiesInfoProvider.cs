using Application.Features.Dividend.Interfaces;
using AutoMapper;
using Domain.Entities;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Providers.Morningstar;

public class SecuritiesInfoProvider : ISecuritiesInfoProvider
{
    private readonly IMapper _mapper;
    private readonly ILogger<SecuritiesInfoProvider> _logger;
    private readonly TimeSpan _queryDelay = TimeSpan.FromSeconds(1);
    
    private const int FirstPage = 1;
    private const int PageSize = 100;
    private const string AsxUniverseId = "E0EXG$XASX";
    private const string Endpoint = "https://graphapi.prd.morningstar.com.au/graphql";

    public SecuritiesInfoProvider(IMapper mapper, ILogger<SecuritiesInfoProvider> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<List<Security>> GetSecuritiesInfo(CancellationToken token)
    {
        var currentPageNumber = FirstPage;
        var totalItems = int.MaxValue;
        var securities = new List<Security>();
            
        using var client = new GraphQLHttpClient(Endpoint, new SystemTextJsonSerializer());

        while (HasMorePages(currentPageNumber, totalItems))
        {
            _logger.LogInformation("Retrieving securities information for universe id: {UniverseId}, page: {PageNumber}",
                AsxUniverseId, currentPageNumber);
            
            var paginatedResponse = await GetPaginatedSecurities(
                client, currentPageNumber, token);

            var mappedSecurities = _mapper.Map<List<Security>>(paginatedResponse.Securities);
            securities.AddRange(mappedSecurities);
            totalItems = paginatedResponse.Total;
            currentPageNumber++;

            if (HasMorePages(currentPageNumber, totalItems))
                await Task.Delay(_queryDelay, token);
        }
        
        _logger.LogInformation("Retrieved securities information for {SecuritiesCount} securities",
            securities.Count);
        
        return securities; 
    }
    
    private static async Task<V1.Api.PaginatedSecurities> GetPaginatedSecurities(IGraphQLClient client, int page, CancellationToken token)
    {
        var arguments = new V1.QueryBuilder.Arguments(DateTime.Today, TimeSpan.FromDays(30), AsxUniverseId, page, PageSize);
        var response = await client.SendQueryAsync<V1.Api.GetUpComingDividendResponse>(
            new GraphQLRequest { Query = $"query {V1.QueryBuilder.BuildUpComingDividendQuery(arguments)}" }, token);

        return response.Data.PaginatedSecurities;
    }

    private static bool HasMorePages(int currentPage, int totalItems) 
        => (currentPage - 1) * PageSize < totalItems;

}