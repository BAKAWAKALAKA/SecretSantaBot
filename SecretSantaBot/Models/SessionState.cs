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
            if (message.Text == "/start" && ( message.User.id == 186463638 || message.User.id == 367265107))
            {
                var buttons = new List<List<Button>>() { new List<Button>() { new Button() { Text="Участвовать", Data= "Участвовать" }, new Button() { Text = "Отказаться", Data = "Отказаться" } } };
                result.Add(new Message()
                {
                    Room = message.Room,
                    Text = "Хо хо хо! Скоро Новый год! Запускаем Тайного Санту!🎅🏻\nЯ распределил для каждого участника своего секретного Санту! \nНажми участвовать и в пятницу я прошепчу тебе на ушко кому дарить подарок.\n\nЛимит подарка: до 1000 рублей\n\nУдаленка диктует свои правила, поэтому предлагаем безопасный обмен:\n🎁Дарим онлайн-подарки;\n🎁Отправляем подарок до ближайшего к подопечному постамата/пункта доставки.\nИ напиши пожелания к подарку – помоги Санте сделать подарок для вас еще более приятным, заодно ты сразу можешь написать оптимальный формат подарка и способ его получения!😊\nДля этого напиши лично Санте @GeheimlichNikolausBot  /wish и укажи свои пожелания.\n\nс 24 по 31 декабря - обмен подарками.🎄\n\n@Alina_Kazarinova - помощник Санты, поможет доставить онлайн подарок или уточнить информацию об адресе доставки. Сохраняя тайну и без лишних вопросов. 😎",
                    keybord = new Keyboard()
                    {
                        Buttons = buttons,
                        Inline = true
                    }
                    
                });

                //var userCount = Extension.TelegramDriver.GetRoomUserCount(message.Room) - 1;
                session.SessionState = new WaitAllResponState(100);
            }
            return result;
        }
    }


    public class WaitAllResponState : ISessionState
    {
        private int _msg;
        private int _startMsg;
        private List<User> acceptUsers;
        private List<User> refuseUsers;
        private int memberCount; 

        public WaitAllResponState(int members)
        {
            acceptUsers = new List<User>();
            refuseUsers = new List<User>();

            memberCount = members;
        }

        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            List<Message> result = new List<Message>() { };

            if (isUserJoin(message))
            {
                using (var db = new SQLiteConnection("Data Source=model.db;"))
                {
                    var data = db.Query<dynamic>($"select * from roomuser where userid = {message.User.id} and roomid = \"{message.Room}\"");
                    if (data.Any())
                    {
                        var res = db.Query<dynamic>($"UPDATE roomuser SET choice = 1 where userid = {message.User.id} and roomid = \"{message.Room}\"");
                    }
                    else
                    {
                      
                            var res = db.Query<dynamic>($"INSERT INTO roomuser (userid, roomid, choice) VALUES ({message.User.id}, {message.Room}, 1);");
                        
                    }
                }
                if (acceptUsers.Any(q => q.id == message.User.id))
                {
                    
                    result.Add(new Message()
                    {
                        callback_query_id = message.CommandId,
                        show_alert = false,
                        Text = "Записал в список хороших детей!"
                    });
                }
                else
                {
                    
                    if (refuseUsers.Any(q => q.id == message.User.id))
                    {
                        refuseUsers.Remove(refuseUsers.First(q => q.id == message.User.id));
                    }
                    acceptUsers.Add(message.User);
                    result.Add(new Message()
                    {
                        callback_query_id = message.CommandId,
                        show_alert = false,
                        Text = "Записал в список хороших детей!"
                    });
                    result.Add(new Message() { Room = message.Room, Text = $"@{message.User.name} я тебя услышал" });
                }
            }
            else
            {
                if (isUserRefuse(message))
                {
                    using (var db = new SQLiteConnection("Data Source=model.db;"))
                    {
                        var data = db.Query<dynamic>($"select * from roomuser where userid = {message.User.id} and roomid = \"{message.Room}\"");
                        if (data.Any())
                        {
                            var res = db.Query<dynamic>($"UPDATE roomuser SET choice = 2 where userid = {message.User.id} and roomid = \"{message.Room}\"");
                        }
                        else
                        {

                            var res = db.Query<dynamic>($"INSERT INTO roomuser (userid, roomid, choice) VALUES ({message.User.id}, {message.Room}, 2);");

                        }
                    }
                    if (refuseUsers.Any(q=>q.id==message.User.id))
                    {
                       
                        result.Add(new Message()
                        {
                            callback_query_id = message.CommandId,
                            show_alert = false,
                            Text = "Записал в список плохишей!"
                        });
                    }
                    else
                    {
                      
                        if (acceptUsers.Any(q=>q.id == message.User.id))
                        {
                            acceptUsers.Remove(acceptUsers.First(q => q.id == message.User.id));
                        }
                        refuseUsers.Add(message.User);
                        result.Add(new Message()
                        {
                            callback_query_id = message.CommandId,
                            show_alert = false,
                            Text = "Записал в список плохишей!"
                        });
                        result.Add(new Message() { Room = message.Room, Text = $"@{message.User.name} я тебя понял" });
                    }
                }
            }

            if (isFinishCommand(message) || memberCount <= acceptUsers.Count + refuseUsers.Count)
            {
                // respond all and go form this state
                var buttons = new List<List<Button>>() { new List<Button>() { new Button() { Text = "Чей я Санта?", Data = "Узнать" } } };
                result.Add(new Message()
                {
                    Room = message.Room,
                    Text = "Ура! Пора узнать чей ты секретный Санта!",
                    keybord = new Keyboard()
                    {
                        Buttons = buttons,
                        Inline = true
                    }

                });
                session.SessionState = new GetResultState(acceptUsers);
                return result;
            }
             return result;
        }

        private bool isUserJoin(Message msg)
        {
            return msg.Command == "Участвовать";
        }

        private bool isUserRefuse(Message msg)
        {
            return msg.Command == "Отказаться";
        }

        private bool isFinishCommand(Message message)
        {
            return message.Text == "/finish" && (message.User.id == 186463638 || message.User.id == 367265107);
        }
    }

    public class GetResultState : ISessionState
    {
        Dictionary<int, User> UserResults;

        public GetResultState(List<User> results)
        {
            UserResults = new Dictionary<int, User>();
            var list = Extension.Rand(results.Count);
            var _list = results.ToArray();
            var i = 0;
            foreach (var res in results)
            {
                UserResults.Add(res.id, _list[list[i]]);
                i++;
            }
        }
        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            List<Message> result = new List<Message>() { };
            
            if(isRestartCommand(message))
            {
                var start = new StartSessionState();
                message.Text = "/start";
                var res = start.NextState(message, session);
                result.AddRange(res);
            }
            else
            {
                if (isGetResultCommand(message))
                {
                    if (UserResults.ContainsKey(message.User.id))
                    {
                        var _text = $"Ты секртеный Санта для @{UserResults.GetValueOrDefault(message.User.id).name}";
                        var subtext = "";
                        try
                        {
                            using (var db = new SQLiteConnection("Data Source=model.db;"))
                            {
                                var data = db.Query<dynamic>($"select * from user where id={UserResults.GetValueOrDefault(message.User.id).id};");
                                if (data.Any() && data.First().wish != null)
                                {
                                   subtext += $".Его(ее) пожелание:\n{data.First().wish}";
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("не получилось взять из базы данные");
                        }
                        if ((_text+subtext).Length<200)
                        {
                            _text += subtext;
                        }
                        else
                        {
                            _text += ".Его(ее) пожелание очень длинное. Увы, я не могу его прошептать. Спроси моего помощника!";
                        }
                        result.Add(new Message()
                        {
                            callback_query_id = message.CommandId,
                            show_alert = true,
                            Text = _text
                        });
                    }
                    else
                    {
                        result.Add(new Message()
                        {
                            callback_query_id = message.CommandId,
                            show_alert = false,
                            Text = $"Ты ж не участвовал!"
                        });
                    }
                }
            }

            return result;
        }

        private bool isRestartCommand(Message msg)
        {
            return msg.Text == "/start";
        }

        private bool isGetResultCommand(Message msg)
        {
            return msg.Command == "Узнать";
        }
    }

    public class WishState : ISessionState
    {
        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            var results = new List<Message>();
            if (!string.IsNullOrWhiteSpace(message.Text))
            {
                using (var db = new SQLiteConnection("Data Source=model.db;"))
                {
                    var data = db.Query<dynamic>($"select * from user where id={message.User.id};");
                    if (data.Any())
                    {
                        var res = db.Query<dynamic>($"UPDATE user SET wish = \"{message.Text}\" WHERE id={message.User.id};");
                    }
                    else
                    {
                        var res = db.Query<dynamic>("INSERT INTO user (id, wish)" +
                                                    $"VALUES ({message.User.id}, \"{message.Text}\");");
                    }
                }
                if(message.Text.Length < 200)
                {
                    results.Add(new Message()
                    {
                        Room = message.Room,
                        Text = $"\"{message.Text}\"\n\nЗаписал!Если нужно что-то поменять - просто пришли /wish",
                    });
                    session.SessionState = null;
                }
                else
                {
                    results.Add(new Message()
                    {
                        Room = message.Room,
                        Text = $"Сообщение слишком длинное! напиши короче",
                    });
                }
            }
            return results;
        }
    }
}