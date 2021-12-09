using SecretSantaBot;
using Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Models;

namespace SecretSantaService.Implementation
{
    public class TelegramDriver : IDriver
    {
        private Telegram.Telegram _telegram;
        public TelegramDriver(Telegram.Telegram telegram)
        {
            _telegram = telegram;
        }

        public IEnumerable<SecretSantaBot.Message> GetUpdates()
        {
            try
            {
                var results = _telegram.GetUpdates();
                // Console.WriteLine($"cur upd: {results.Count()}");
                var messages = results.Where(q=>q.message!=null).Select(q => q.message.CreateBotMessage()).ToList();
                messages.AddRange(results.Where(q => q.callback_query != null).Select(q => q.callback_query.CreateBotMessage()));
                // Console.WriteLine($"mes: {messages.Count()}");
                return messages;
            }
            catch(Exception exception)
            {
                throw new Exception("can't connect to telegram server.");
            }
        }

        public bool SendResponse(SecretSantaBot.Message response)
        {
            try
            {
                if (response.callback_query_id == null)
                {
                    var message = response.CreateTelegramReplay();
                    var result = _telegram.SendMessage(message);
                    return result != null;
                }
                else
                {
                    var mes = response.CreateTelegramCallbackQueryReplay();
                    var result = _telegram.SendAnswerCalbackQuery(mes);
                    return result != null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("can't connect to telegram server.");
            }
        }

        public int GetRoomUserCount(string chatId)
        {
            try
            {
                var results = _telegram.GetChatCount(chatId);
                return results;
            }
            catch(Exception exception)
            {
                throw new Exception("can't connect to telegram server.");
            }
        }

        public User GetUser(int id, string chatId)
        {
            try
            {
                var from = _telegram.GetChatMember(chatId, id);
                var user = new User()
                {
                    id = from.id,
                    name = $"{from.username} {from.first_name} {from.last_name}"

                };

                return user;
            }
            catch (Exception exception)
            {
                throw new Exception("can't connect to telegram server.");
            }
        }
    }
}
