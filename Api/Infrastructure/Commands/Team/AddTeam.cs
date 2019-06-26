using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;


namespace FirmaBudowlana.Infrastructure.Commands.Team
{
    public class AddTeam: ICommand
    {
        public TeamDTO Team { get; set; }
    }
}
