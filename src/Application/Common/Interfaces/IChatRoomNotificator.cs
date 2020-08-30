
using System.Threading.Tasks;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Common.Interfaces
{

    public interface IChatRoomNotificator
    {

        Task MessageRead(ChatMessage message);

    }

}