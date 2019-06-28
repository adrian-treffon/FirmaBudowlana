using Komis.Infrastructure.Commands;
using System;


namespace FirmaBudowlana.Infrastructure.Commands.Team
{
    public class DeleteTeam : ICommand
    {
        public Guid TeamID { get; set; }
    }
}
