using rentasgt.Domain.Entities;
using System.Text;
using System.Threading.Tasks;

namespace rentasgt.Application.Common.Interfaces
{
    public interface IChatRoomNotifier
    {

        Task MessageRead(ChatMessage msg);

    }
}
