using AutoMapper;
using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace FirmaBudowlana.Infrastructure.Mapper
{
    public class ComparisonOrderDTOMappingProfile : IMappingAction<Order, ComparisonOrderDTO>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly DBContext _context;

        public ComparisonOrderDTOMappingProfile(IMapper mapper, IOrderRepository orderRepository, IWorkerRepository workerRepository,
            ITeamRepository teamRepository, IPaymentRepository paymentRepository, DBContext context)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
            _teamRepository = teamRepository;
            _paymentRepository = paymentRepository;
            _context = context;
        }

      
        public void Process(Order source, ComparisonOrderDTO destination)
        {
            var payments = _paymentRepository.GetAllAsync().Result.Where(x => x.OrderID == source.OrderID).ToList();

            if (payments.Any())
            {
                destination.Payments = payments;
                destination.Paid = true;
            }
            else destination.Payments = null;


            var teams =  _context.OrderTeam.Where(x => x.OrderID == source.OrderID).ToList();

            foreach (var team in teams)
            {
                var jeden =_teamRepository.GetAsync(team.TeamID).Result;
                var dwa = _mapper.Map<TeamDTO>(jeden);
                destination.Teams.Add(dwa);
            }
        }
    }
}
