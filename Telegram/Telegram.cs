using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telegram.Models;

namespace Telegram
{
    public class Telegram
    {
        private string _base;
        private string requestTemplate = @"https://api.telegram.org/bot{0}";
        private long _lastUpdate;
        private long _offset;
        private bool isFirstTime; // костыль ибо телега говно которая не может нормлаьно время проставлять для колбеков

        public Telegram()
        {
            try
            {
                var _botToken = File.ReadAllText(AppContext.BaseDirectory + "token.txt");
                _base = string.Format(requestTemplate, _botToken);
                var str = File.ReadAllText(AppContext.BaseDirectory + "lastdate.txt");
                if (!long.TryParse(str, out _lastUpdate))
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
            return Execute<Message>("sendMessage", message);
        }

        public Message SendAnswerCalbackQuery(answerCallbackQuery message)
        {
            return Execute<Message>("answerCallbackQuery", message);
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
               
                var result = (_offset == null) ? Execute<Updates>("getUpdates"): Execute<Updates>($"getUpdates?offset={_offset+1}");
                if (result?.ok ?? false)
                {
                    if (!isFirstTime)
                    {
                        if(result.result.Count()>99) _offset = result.result.OrderBy(q => q.update_id).Last().update_id;
                        Console.WriteLine($"raw upd: {result.result.Count}");
                        var results = result.result.Where(q => q.message?.date > _lastUpdate).ToList();
                        results.AddRange(result.result.Where(q => q.callback_query?.message?.date > _lastUpdate));
                        if (result.result.Any())
                        {
                            _lastUpdate = result.result.OrderBy(q => q.update_id).Last().update_id;
                        }
                        else
                        {
                            _lastUpdate = 0;
                        }
                        isFirstTime = true;
                        return results;
                    }
                    else
                    {
                        if (result.result.Count() > 99) _offset = result.result.OrderBy(q => q.update_id).Last().update_id;
                        Console.WriteLine($"raw upd: {result.result.Count}");
                        var results = result.result.Where(q => q.update_id> _lastUpdate).ToList();
                        if (results.Any())
                        {
                            _lastUpdate = results.OrderBy(q => q.update_id).Last().update_id;
                        }
                        return results;
                    }
                }
                return new List<Update>();
            }
            catch (Exception e)
            {
                Console.WriteLine("tl err");
                throw;
            }
        }

        class updateComparer : IComparer<Update>
        {
            public int Compare(Update p1, Update p2)
            {
                if ((p1.message?.date ?? p1.callback_query.message.date )>(p2.message?.date ?? p2.callback_query.message.date))
                    return 1;
                else if ((p1.message?.date ?? p1.callback_query.message.date) < (p2.message?.date ?? p2.callback_query.message.date))
                    return -1;
                else
                    return 0;
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

        //todo change!!!it's wrong
        public int GetChatCount(string chatId)
        {
            var result = Execute<ChatCountResponse>($"getChatCount?chat_id={chatId}");
            return result.result;
        }

        public From GetChatMember(string chatId, int userId)
        {
            try
            {
                var result = Execute<UserMemberResponse>($"getChatMember?chat_id={chatId}&user_id={userId}");
                if (result.ok)
                {
                    return result.result.user;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public T Execute<T>(string cmd, IBody sendingMessage = null)
        {
            var client = new RestClient($"{_base}/{cmd}");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            if (sendingMessage != null)
            {
                var jsonString = JsonConvert.SerializeObject(sendingMessage,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            }

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
