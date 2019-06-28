using Komis.Infrastructure.Commands;
using System;


namespace FirmaBudowlana.Infrastructure.Commands.Worker
{
    public class DeleteWorker : ICommand
    {
        public Guid WorkerID { get; set; }
    }
}
