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
        long? startedUserId;
        public WaitAllResponState(long? userId)
        {
            startedUserId = userId;
        }

        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            List<Message> result = new List<Message>() { };

            if (isUserJoin(message))
            {
                SetToDB(message, true);
                result.Add(new Message()
                {
                    callback_query_id = message.CommandId,
                    show_alert = false,
                    Text = "Записал в список хороших детей!"
                });
            }
            else if (isUserRefuse(message))
            {
                SetToDB(message, false);
                result.Add(new Message()
                {
                    callback_query_id = message.CommandId,
                    show_alert = false,
                    Text = "Хорошо, я не буду тебя записывать"
                });
            }

            if (isFinishCommand(message))
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
                session.SessionState = new GetResultState(message.Room);
                return result;
            }
            return result;
        }

        private void SetToDB (Message message, bool join)
        {
            using (var db = new SQLiteConnection("Data Source=model.db;"))
            {
                var joinInt = join ? 1 : 2;
                var data = db.Query<dynamic>($"select * from roomuser where userid = {message.User.id} and roomid = \"{message.Room}\"");
                if (data.Any())
                {
                    db.Query<dynamic>($"UPDATE roomuser SET choice = {joinInt} where userid = {message.User.id} and roomid = \"{message.Room}\"");
                }
                else
                {
                    db.Query<dynamic>($"INSERT INTO roomuser (userid, roomid, choice) VALUES ({message.User.id}, {message.Room}, {joinInt});");
                    var user = db.Query<dynamic>($"select * from user where id = {message.User.id}");
                    if (!user.Any())
                    {
                        db.Query<dynamic>($"INSERT INTO user (id, nickname, lastname, firstname) VALUES ({message.User.id}, \"{message.User.Nickname}\", \"{message.User.LastName}\", \"{message.User.FirstName}\");");
                    }
                }
            }

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
            return message.Text == "/finish" && startedUserId == message.User.id;
        }
    }

}
