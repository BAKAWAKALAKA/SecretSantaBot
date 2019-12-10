using SecretSantaBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot
{
    public class RoomSession
    {
        public List<User> Selected { get; set; }
        public int Room { get; private set; }
        public ISessionState SessionState { get; set; }

        public RoomSession(int room)
        {
            Room = room;
            Selected = new List<User> ();
            SessionState = new StartSessionState();
        }

        public IEnumerable<Message> Next(Message message)
        {
            return SessionState.NextState(message, this);
        }
    }
}
