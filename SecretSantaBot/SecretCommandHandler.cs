using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSantaBot
{
    public class SecretCommandHandler : ICommandHandler
    {
        List<RoomSession> _sessions;

        public SecretCommandHandler()
        {
            _sessions = new List<RoomSession>();
        }

        public bool CanRespond(Message message)
        {
            return message.RoomType != "private" ? true : true;
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
                Console.WriteLine($"==============new room, room count {_sessions.Count}");
                return newRoomSession.Next(message);
            }
        }
    }
}
