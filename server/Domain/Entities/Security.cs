namespace Domain.Entities;

public class Security
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ticker { get; set; }
    public string SecurityType { get; set; }
    public string ExchangeCode { get; set; }
    public int FrankedRate { get; set; }
    public string ExDate { get; set; }
    public string PayDate { get; set; }
    public double DivCashAmount { get; set; }
    public string Currency { get; set; }
    public string Reference { get; set; }
}