using AutoMapper;
using RyHCarRental.API.DTOs.Request;
using RyHCarRental.API.DTOs.Response;
using RyHCarRental.Domain.Entities;

namespace RyHCarRental.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<CustomerRequestDto, Customer>();

            CreateMap<VehicleType, VehicleTypeResponseDto>();
            CreateMap<VehicleTypeRequestDto, VehicleType>();

            CreateMap<Branch, BranchResponseDto>();
            CreateMap<BranchRequestDto, Branch>();

            CreateMap<Vehicle, VehicleResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType != null ? src.VehicleType.Name : null))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : null));
            CreateMap<VehicleRequestDto, Vehicle>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleType, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore());
            CreateMap<VehicleUpdateRequestDto, Vehicle>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleType, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore());

            CreateMap<Rental, RentalResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.RentalDetails, opt => opt.MapFrom(src => src.RentalDetails));

            CreateMap<RentalDetail, RentalDetailResponseDto>()
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal));

            CreateMap<RentalRequestDto, Rental>()
                .ForMember(dest => dest.RentalDetails, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
        }
    }
}
