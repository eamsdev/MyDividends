using System.Text.Json.Serialization;

namespace Infrastructure.Providers.Morningstar.V1;

public static class Api
{
    public class GetUpComingDividendResponse
    {
        [JsonPropertyName("screener")]
        public PaginatedSecurities PaginatedSecurities { get; set; }
    }
    
    public class PaginatedSecurities
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        
        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        
        [JsonPropertyName("securities")]
        public Security[] Securities { get; set; }
    }
    
    public class Security
    {
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("securityType")]
        public string SecurityType { get; set; }
        
        [JsonPropertyName("exchangeCode")]
        public string ExchangeCode { get; set; }
        
        [JsonPropertyName("frankedRate")]
        public int FrankedRate { get; set; }
        
        [JsonPropertyName("exDate")]
        public string ExDate { get; set; }
        
        [JsonPropertyName("payDate")]
        public string PayDate { get; set; }
        
        [JsonPropertyName("divCashAmount")]
        public double DivCashAmount { get; set; }
        
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}