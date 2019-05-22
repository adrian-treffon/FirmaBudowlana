using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;

namespace FirmaBudowlana.Infrastructure.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ClientOrderDTO, Order>().ReverseMap();
            CreateMap<AdminOrderDTO, Order>().ReverseMap();
        }
    }
}
