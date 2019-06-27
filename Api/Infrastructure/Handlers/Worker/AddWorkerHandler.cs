using AutoMapper;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Worker;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Worker
{
    public class AddWorkerHandler : ICommandHandler<AddWorker>
    {
        private readonly IMapper _mapper;
        private readonly IWorkerRepository _workerRepository;
      
        public AddWorkerHandler(IMapper mapper, IWorkerRepository workerRepository)
        {
            _mapper = mapper;
            _workerRepository = workerRepository;
        }

        public async Task HandleAsync(AddWorker command)
        {
            if (command.Worker == null) throw new ServiceException(ErrorCodes.PustyRequest,"Post request add/worker is empty");

            var worker = _mapper.Map<Core.Models.Worker>(command.Worker);
            worker.WorkerID = Guid.NewGuid();
            await _workerRepository.AddAsync(worker);
        }
    }
}
