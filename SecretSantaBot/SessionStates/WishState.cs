using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretSantaBot.Interfaces;
using System.Data.SQLite;
using Dapper;

namespace SecretSantaBot
{
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
                if (message.Text.Length < 200)
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
