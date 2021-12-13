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
                var buttons = new List<List<Button>>() {
                    new List<Button> () {
                        new Button() { Text ="Участвовать", Data= "Участвовать" },
                        new Button() { Text = "Отказаться", Data = "Отказаться" }
                    },
                    new List<Button> ()
                    {
                        new Button() { Text = "Написать пожелание", URL = @"https://t.me/GeheimlichNikolausBot?start=wish"}
                    }
                };
                result.Add(new Message()
                {
                    Room = message.Room,
                    Text = "Хо хо хо! Скоро Новый год! Запускаем Тайного Санту!🎅🏻\nЯ распределю для каждого участника своего секретного Санту!\nНажми участвовать и в пятницу я прошепчу тебе на ушко кому дарить подарок.\n\nЛимит подарка: до 1000 рублей\n\nВ этом году мы вновь предлагаем безопасный обмен подарками:\n🎁 Дарим онлайн-подарки;\n🎁 Отправляем подарок до ближайшего к подопечному постамата/пункта доставки.\n\nПоэтому обязательно напиши пожелания к подарку и оптимальный способ получения – помоги Санте сделать сюрприз для тебя еще более приятным. ☺️\nДля этого напиши лично Санте @GeheimlichNikolausBot свои пожелания.\n\nс 23 по 31 декабря - обмен подарками.🎄\n\n@Alina_Kazarinova - помощник Санты, поможет доставить онлайн подарок или уточнить информацию об адресе доставки, соблюдая полную конфиденциальность.😎",
                    keybord = new Keyboard()
                    {
                        Buttons = buttons,
                        Inline = true
                    }
                    
                });

                session.SessionState = new WaitAllResponState(message.User.id);
            }
            return result;
        }
    }
    
}