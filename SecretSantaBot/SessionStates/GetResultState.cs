using System;
using System.Collections.Generic;
using System.Linq;
using SecretSantaBot.Interfaces;
using System.Data.SQLite;
using Dapper;

namespace SecretSantaBot
{
    public class GetResultState : ISessionState
    {
        Dictionary<int, User> UserResults;
        RoomSession roomSession;

        public GetResultState(RoomSession roomSession,List<User> results)
        {
            // results = GetFromDb();
            UserResults = new Dictionary<int, User>();
            if (results.Any())
            {
                var list = Extension.Rand(results.Count);
                var _list = results.ToArray();
                var i = 0;
                foreach (var res in results)
                {
                    UserResults.Add(res.id, _list[list[i]]);
                    i++;
                }
                SetResultTodb();
            }
        }

        private List<User> GetFromDb()
        {
            var results = new List<User>();
            using (var db = new SQLiteConnection("Data Source=model.db;"))
            {
                var data = db.Query<dynamic>($"select u.userid, u.firstname, u.lastname, u.nickname from roomuser ru inner left user u on u.userid=ru.userid where ru.roomid={roomSession.Room} and ru.choice = 1;");
                if (data.Any())
                {
                    foreach(var user in data)
                    {
                        results.Add(new User() { id = user.userid, name = $"{user.firstname} {user.lastname} {user.nickname}" });
                    }
                }
                return results;
            }
        }

        private void SetResultTodb()
        {
            using (var db = new SQLiteConnection("Data Source=model.db;"))
            {
                foreach(var user in UserResults)
                {
                    var res = db.Query<dynamic>($"INSERT INTO results (userid, roomid, touserid) VALUES ({user.Key}, {roomSession.Room}, {user.Value.id});");
                }
            }
        }

        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            List<Message> result = new List<Message>() { };

            if (isRestartCommand(message))
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
                        if ((_text + subtext).Length < 200)
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
}
