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
            CreateMap<WorkerDTO, Worker>().ReverseMap();
            CreateMap<TeamDTO, Team>().ReverseMap();
            CreateMap<OrderToPaidDTO, Order>().ReverseMap();
            CreateMap<ComparisonOrderDTO, Order>().ReverseMap();
          
        }
    }
}
