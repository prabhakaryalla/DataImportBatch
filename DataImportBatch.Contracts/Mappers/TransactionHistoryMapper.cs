using System;
using AutoMapper;
using DataImportBatch.Contracts.Models;

namespace DataImportBatch.Contracts.Mappers;

public class TransactionHistoryMapper : Profile
{
    public TransactionHistoryMapper()
    {
        CreateMap<TransactionHistoryRawModel, TransactionHistoryOutputModel>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}
