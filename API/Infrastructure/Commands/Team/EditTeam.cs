using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;

namespace FirmaBudowlana.Infrastructure.Commands.Team
{
    public class EditTeam :ICommand
    {
        public TeamDTO Team { get; set; }
    }
}
