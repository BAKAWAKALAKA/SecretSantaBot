using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSantaBot
{
    public class PrivateRoomCommandHandler : ICommandHandler
    {
        List<RoomSession> _sessions;

        public PrivateRoomCommandHandler()
        {
            _sessions = new List<RoomSession>();
        }
        public bool CanRespond(Message message)
        {
            return message.RoomType == "private" ? true : false;
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
                var results = new List<Message>();
                if (message.Text == "/wish")
                {
                    var newRoomSession = new RoomSession(message.Room, new WishState());
                    _sessions.Add(newRoomSession);
                    results.Add(new Message()
                    {
                        Room = message.Room,
                        Text = $"Напиши свои пожелания, а я сообщу их твоему Секретному Санте.",
                    });
                }
                else
                {
                    if (message.Text == "/help")
                    {
                        results.Add(new Message()
                        {
                            Room = message.Room,
                            Text = "Хо хо хо, привет!\n Я бот для розыгрыша секретного Санты (https://ru.wikipedia.org/wiki/%D0%A2%D0%B0%D0%B9%D0%BD%D1%8B%D0%B9_%D0%A1%D0%B0%D0%BD%D1%82%D0%B0).\n**Доступны следующие команды:**\n/start - создаёт новый розыгрыш (если уже начат розыгрыш нужно сначала его завершить)\n/finish - завершает текущий розыгрыш\n**Ход работы:**\nДобавь меня в чат и нажми /start, когда все проголосуют (или ты нажмешь /finish) я завершу розыгрыш."
                        });
                    }
                }
                return results; 
            }
        }

    }
}
