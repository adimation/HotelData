using AutoMapper;
using HotelData.Data;
using HotelData.DTOs;

namespace HotelData.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<HotelSourceDTO, Hotel>()
                .ForMember(n => n.Id, opt => opt.Ignore())
                .ForMember(n => n.ExternalId, opt => opt.MapFrom(x => x.Id))
                .ForMember(n => n.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(n => n.SupplierId, opt => opt.MapFrom(x => x.SupplierId))
                .ForMember(n => n.SupplierName, opt => opt.MapFrom(x => x.SupplierName))
                .ForMember(n => n.Country, opt => opt.MapFrom(x => x.Address.Country))
                .ForMember(n => n.Region, opt => opt.MapFrom(x => x.Address.Region))
                .ForMember(n => n.City, opt => opt.MapFrom(x => x.Address.City))
                .ForMember(n => n.Latitude, opt => opt.MapFrom(x => x.Address.Latitude))
                .ForMember(n => n.Longitude, opt => opt.MapFrom(x => x.Address.Longitude));

            CreateMap<HotelDTO, Hotel>().ReverseMap();
        }
    }
}
