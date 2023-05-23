using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class SecurityEntityConfiguration : IEntityTypeConfiguration<Security>
{
    public void Configure(EntityTypeBuilder<Security> builder)
    {
        builder.ToTable("securities");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Ticker)
            .HasColumnName("ticker")
            .IsRequired();
        
        builder.Property(x => x.SecurityType)
            .HasColumnName("security_type")
            .IsRequired();
        
        builder.Property(x => x.ExchangeCode)
            .HasColumnName("exchange_code")
            .IsRequired();
        
        builder.Property(x => x.FrankedRate)
            .HasColumnName("franked_rate")
            .IsRequired();
        
        builder.Property(x => x.ExDate)
            .HasColumnName("ex_date")
            .IsRequired();
        
        builder.Property(x => x.PayDate)
            .HasColumnName("pay_date")
            .IsRequired();
        
        builder.Property(x => x.DivCashAmount)
            .HasColumnName("div_cash_amount")
            .IsRequired();
        
        builder.Property(x => x.Currency)
            .HasColumnName("currency")
            .IsRequired();
        
        builder.HasIndex(x => x.Reference)
            .IsUnique();
        builder.Property(x => x.Reference)
            .HasColumnName("reference")
            .IsRequired();
    }
}