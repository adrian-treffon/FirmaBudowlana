using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;


namespace FirmaBudowlana.Infrastructure.Commands.Worker
{
    public class EditWorker : ICommand
    {
        public WorkerDTO Worker { get; set; }
    }
}
