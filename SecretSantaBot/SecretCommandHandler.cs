using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSantaBot
{
    public class SecretCommandHandler : ICommandHandler
    {
        List<RoomSession> _sessions;

        public bool CanRespond(Message message)
        {
            //todo if bot join to room
            var session = _sessions.FirstOrDefault(q => q._room == message.Room);
            return (session != null) ? true : false;
        }

        public IEnumerable<Message> Respond(Message message)
        {
            var session = _sessions.FirstOrDefault(q => q._room == message.Room);
            if (session != null)
            {
                return session.Next(message);
            }
            else
            {
                var newRoomSession = new RoomSession(message.Room,0);
                _sessions.Add(newRoomSession);
                return newRoomSession.Next(message);
            }
        }

        
    }
}
