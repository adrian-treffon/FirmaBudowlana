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

            CreateMap<Order, OrderToPaidDTO>();

            CreateMap<Order, ComparisonOrderDTO>().AfterMap<ComparisonOrderDTOMappingProfile>();


            CreateMap<Worker, WorkerDTO>().AfterMap<WorkerDTOMappingProfile>();
            CreateMap<WorkerDTO, Worker>();
               

            CreateMap<Team, TeamDTO>().AfterMap<TeamDTOMappingProfile>();
            CreateMap<TeamDTO, Team>().AfterMap<TeamMappingProfile>();

            CreateMap<User, ComparisonUserDTO>();

         
        }
    }
}
