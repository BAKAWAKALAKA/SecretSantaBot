using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretSantaBot.Interfaces;
using System.Data.SQLite;
using Dapper;

namespace SecretSantaBot
{
    public class StartSessionState: ISessionState
    {
        public StartSessionState()
        {
            
        }

        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            var result = new List<Message>();
            if (message.Text == "/start")
            {
                var buttons = new List<List<Button>>() { new List<Button>() { new Button() { Text="Участвовать", Data= "Участвовать" }, new Button() { Text = "Отказаться", Data = "Отказаться" } } };
                result.Add(new Message()
                {
                    Room = message.Room,
                    Text = "Хо хо хо! Скоро Новый год! Запускаем Тайного Санту!🎅🏻\nЯ распределю для каждого участника своего секретного Санту! \nНажми участвовать и потом я прошепчу тебе на ушко кому дарить подарок.\nИ напиши пожелания к подарку – помоги Санте сделать подарок для вас еще более приятным, заодно ты сразу можешь написать оптимальный формат подарка и способ его получения!😊\nДля этого напиши лично Санте @GeheimlichNikolausBot  /wish и укажи свои пожелания.\n😎",
                    keybord = new Keyboard()
                    {
                        Buttons = buttons,
                        Inline = true
                    }
                    
                });

                //var userCount = Extension.TelegramDriver.GetRoomUserCount(message.Room) - 1;
                session.SessionState = new WaitAllResponState(1000);
            }
            return result;
        }
    }
    
}