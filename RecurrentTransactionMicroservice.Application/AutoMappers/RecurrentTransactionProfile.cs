using AutoMapper;



namespace RecurrentTransactionMicroservice.Core.AutoMappers;

public class RecurrentTransactionProfile : Profile
{
    public RecurrentTransactionProfile()
    {

        CreateMap<CreateRecurrentTransactionRequest, RecurrentTransactionMicroservice.Domain.Models.RecurrentTransaction>()
            .ForMember(dest=> dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest=> dest.Interval, opt => opt.MapFrom(src => src.Interval))
            .ForMember(dest=> dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest=> dest.IsDeposit, opt => opt.MapFrom(src => src.IsDeposit))
            .ForMember(dest => dest.BranchId, opt =>opt.MapFrom(src => src.BranchId));
        
        CreateMap<RecurrentTransactionMicroservice.Domain.Models.RecurrentTransaction, RecurrentTransactions>()
            .ForMember(dest=> dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest=> dest.Interval, opt => opt.MapFrom(src => src.Interval))
            .ForMember(dest=> dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest=> dest.IsDeposit, opt => opt.MapFrom(src => src.IsDeposit))
            .ForMember(dest => dest.BranchId, opt =>opt.MapFrom(src => src.BranchId));

    }
}