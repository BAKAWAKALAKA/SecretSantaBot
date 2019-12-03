using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot.Interfaces
{

    public interface ISessionState
    {
        IEnumerable<Message> NextState(Message message,RoomSession session);
    }

}
