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