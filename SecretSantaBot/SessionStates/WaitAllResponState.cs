using System;
using System.Collections.Generic;
using System.Linq;
using SecretSantaBot.Interfaces;
using System.Data.SQLite;
using Dapper;

namespace SecretSantaBot
{
    public class WaitAllResponState : ISessionState
    {
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
                    if (refuseUsers.Any(q => q.id == message.User.id))
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

                        if (acceptUsers.Any(q => q.id == message.User.id))
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
            return message.Text == "/finish";
        }
    }

}
