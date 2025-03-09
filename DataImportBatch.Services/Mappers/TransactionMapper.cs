using AutoMapper;
using DataImportBatch.Data.Models;
using DataImportBatch.Services.Models;

namespace DataImportBatch.Services.Mappers;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<ProductData, Product>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ListPrice, opt => opt.MapFrom(src => GetDecimalValue(src.ListPrice)));


        CreateMap<TransactionHistoryData, TransactionHistory>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.TransactionDate)))
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src =>  src.TransactionType))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => Convert.ToInt32(src.Quantity)))
            .ForMember(dest => dest.ActualCost, opt => opt.MapFrom(src => GetDecimalValue(src.ActualCost)))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.ModifiedDate)));
    }

    private decimal GetDecimalValue(string price)
    {
        if (decimal.TryParse(price, out decimal result))
            return result;
        return 0;
    }
}
