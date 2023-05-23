using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Providers.Morningstar.V1;

public class SecurityMappingProfile : Profile
{
    public SecurityMappingProfile()
    {
        CreateMap<Api.Security, Security>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
            .ForMember(dest => dest.Ticker, opt => opt.MapFrom(source => source.Ticker))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(source => source.Currency))
            .ForMember(dest => dest.SecurityType, opt => opt.MapFrom(source => source.SecurityType))
            .ForMember(dest => dest.ExchangeCode, opt => opt.MapFrom(source => source.ExchangeCode))
            .ForMember(dest => dest.FrankedRate, opt => opt.MapFrom(source => source.FrankedRate))
            .ForMember(dest => dest.ExDate, opt => opt.MapFrom(source => source.ExDate))
            .ForMember(dest => dest.PayDate, opt => opt.MapFrom(source => source.PayDate))
            .ForMember(dest => dest.DivCashAmount, opt => opt.MapFrom(source => source.DivCashAmount))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => $"{source.ExchangeCode}:{source.Ticker}"));
    }
}