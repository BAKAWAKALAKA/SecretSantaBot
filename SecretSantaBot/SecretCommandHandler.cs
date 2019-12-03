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
            var session = _sessions.FirstOrDefault(q => q.Room == message.Room);
            return (session != null) ? true : false;
        }

        public IEnumerable<Message> Respond(Message message)
        {
            var session = _sessions.FirstOrDefault(q => q.Room == message.Room);
            if (session != null)
            {
                var result = session.Next(message);
                if (session.SessionState == null)
                {
                    _sessions.Remove(session);
                }
                return result;
            }
            else
            {
                var newRoomSession = new RoomSession(message.Room);
                _sessions.Add(newRoomSession);
                return newRoomSession.Next(message);
            }
        }

        
    }
}
