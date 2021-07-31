using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot
{
    public class HelpCommandHandler : ICommandHandler
    {
        public bool CanRespond(Message message)
        {
            return (message.Text=="/help")? true: false;
        }

        public IEnumerable<Message> Respond(Message message)
        {
            var result = new List<Message>();

            result.Add(new Message() {
                Room = message.Room,
                Text = "Хо хо хо, привет!\n Я бот для розыгрыша секретного Санты (https://ru.wikipedia.org/wiki/%D0%A2%D0%B0%D0%B9%D0%BD%D1%8B%D0%B9_%D0%A1%D0%B0%D0%BD%D1%82%D0%B0).\n**Доступны следующие команды:**\n/start (в чате) - создаёт новый розыгрыш (если уже начат розыгрыш нужно сначала его завершить)\n/finish (в чате) - завершает текущий розыгрыш\n/wish (в привате с ботом) - укажи свои пожелания.\n**Ход работы:**\nДобавь меня в чат и нажми /start, когда все проголосуют (или ты нажмешь /finish) я завершу розыгрыш."
            });

            return result;
        }
    }
}
