using SecretSantaBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot
{
    public class RoomSession
    {
        public int Room { get; private set; }
        public ISessionState SessionState { get; set; }

        public RoomSession(int room)
        {
            Room = room;
        }

        public IEnumerable<Message> Next(Message message)
        {
            return SessionState.NextState(message, this);
        }
    }
}
