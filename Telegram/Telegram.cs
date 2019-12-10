using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Models;

namespace Telegram
{
    public class Telegram
    {
        private string proxy = "217.182.92.162";
        private int port = 3128;
        private string _base;
        private string requestTemplate = @"https://api.telegram.org/bot{0}";
        private int _lastUpdate;

        public Telegram()
        {
            try
            {
                var _botToken = File.ReadAllText(AppContext.BaseDirectory + "token.txt");
                _base = string.Format(requestTemplate, _botToken);
                var str = File.ReadAllText(AppContext.BaseDirectory + "lastdate.txt");
                if (!int.TryParse(str, out _lastUpdate))
                {
                    var time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    var current = DateTime.Now.Subtract(new TimeSpan(3, 0, 0)); // ибо рашка
                    var diff = current - time;
                    _lastUpdate = (int)diff.TotalSeconds;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Message SendMessage(SendMessage message)
        {
            throw new NotImplementedException();
        }

        public bool CheckConnection()
        {
            try
            {
                var result = Execute<Check>("getMe");
                return result.ok;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<Update> GetUpdates()
        {
            try
            {
                var result = Execute<Updates>("getUpdates");
                if (result.ok)
                {
                    return result.result;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ChatInfo GetChatInfo(int chatId)
        {

            try
            {
                var result = Execute<ChatInfoResponse>($"getChatInfo?chat_id={chatId}");
                if (result.ok)
                {
                    return result.chat_info;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool ChangeProxy(string proxy)
        {
            throw new NotImplementedException();
        }

        //todo change!!!it's wrong
        public int GetChatCount(int chatId)
        {
            var result = Execute<ChatCountResponse>($"getChatCount?chat_id={chatId}");
            return result.count;
        }

        public From GetChatMember(int chatId, int userId)
        {
            try
            {
                var result = Execute<UserMemberResponse>($"getChatMember?chat_id={chatId}&user_id={userId}");
                if (result.ok)
                {
                    return result.user;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public T Execute<T>(string cmd, SendMessage sendingMessage = null)
        {
            var client = new RestClient($"{_base}/{cmd}");
            client.Proxy = new System.Net.WebProxy(proxy, port);

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(sendingMessage);

            var responce = client.Execute(request);
            if (responce.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<T>(responce.Content); //change response to using list with generic class  for calling  Execute<Response<SomeGenericClass>>
                return result;
            }
            else
            {
                return default(T);
            }
        }
    }
}
