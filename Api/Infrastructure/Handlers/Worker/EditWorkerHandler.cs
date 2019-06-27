using AutoMapper;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.Worker;
using FirmaBudowlana.Infrastructure.Exceptions;
using Komis.Infrastructure.Commands;
using System;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.Worker
{
    public class EditWorkerHandler : ICommandHandler<EditWorker>
    {
        private readonly IMapper _mapper;
        private readonly IWorkerRepository _workerRepository;
     
        public EditWorkerHandler(IWorkerRepository workerRepository,IMapper mapper)
        {
            _workerRepository = workerRepository;
            _mapper = mapper;
        }

        public async  Task HandleAsync(EditWorker command)
        {
            if (command.Worker == null) throw new ServiceException(ErrorCodes.PustyRequest,"Post request edit/worker is empty");

            var workerFromDB = await _workerRepository.GetAsync(command.Worker.WorkerID);

            if (workerFromDB == null) throw new ServiceException(ErrorCodes.Nieznaleziono,$"Nie można znaleźć pracownika w bazie danych");

            await _workerRepository.UpdateAsync(_mapper.Map<Core.Models.Worker>(command.Worker));
        }
    }
}
