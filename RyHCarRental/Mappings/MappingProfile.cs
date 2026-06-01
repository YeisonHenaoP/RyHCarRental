using AutoMapper;
using RyHCarRental.API.DTOs;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;

namespace RyHCarRental.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerCreateDto, Customer>();

            CreateMap<VehicleType, VehicleTypeDto>();
            CreateMap<VehicleTypeCreateDto, VehicleType>();

            CreateMap<Branch, BranchDto>();
            CreateMap<BranchCreateDto, Branch>();

            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType != null ? src.VehicleType.Name : null))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : null));
            CreateMap<VehicleCreateDto, Vehicle>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleType, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore());
            CreateMap<VehicleUpdateDto, Vehicle>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleType, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore());

            // Rental mappings
            CreateMap<Rental, RentalDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.RentalDetails, opt => opt.MapFrom(src => src.RentalDetails));

            // RentalDetail mappings
            CreateMap<RentalDetail, RentalDetailDto>()
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal));

            // RentalCreateDto to Rental (ignoramos propiedades que se calculan)
            CreateMap<RentalCreateDto, Rental>()
                .ForMember(dest => dest.RentalDetails, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
        }
    }
}