using SecretSantaBot;
using Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxyParser;
using Telegram.Models;

namespace SecretSantaBotConsole.Implementation
{
    public class TelegramDriver : IDriver
    {
        private Telegram.Telegram _telegram;
        private ProxyServerGrabberFromFile _proxy;
        private int _lastTime; // maybe be using for getupdate method
        public TelegramDriver(Telegram.Telegram telegram, ProxyServerGrabberFromFile proxy)
        {
            _telegram = telegram;
            _proxy = proxy;


        }

        public IEnumerable<Message> GetUpdates()
        {
            try
            {
                var results = _telegram.GetUpdates();
                var messages = results.Select(q => q.message.CreateBotMessage());
                return messages;
            }
            catch(Exception exception)
            {
                var proxy = _proxy.current_proxy;
                while (proxy!=null)
                {
                    _telegram.ChangeProxy(proxy);
                    if (_telegram.CheckConnection())
                    {
                        return new List<Message>();
                    }
                    proxy = _proxy.current_proxy;
                }
                throw new Exception("can't connect to telegram server. Proxy didn't help");
            }
        }

        public bool SendResponse(Message response)
        {
            try
            {
                var message = response.CreateTelegramReplay();
                var result = _telegram.SendMessage(message);
                return result != null;
            }
            catch (Exception exception)
            {
                var proxy = _proxy.current_proxy;
                while (proxy != null)
                {
                    _telegram.ChangeProxy(proxy);
                    if (_telegram.CheckConnection())
                    {
                        return false;
                    }
                    proxy = _proxy.current_proxy;
                }
                throw new Exception("can't connect to telegram server. Proxy didn't help");
            }
        }

        private int GetRoomUserCount(int chatId)
        {
            try
            {
                var results = _telegram.GetChatCount(chatId);
                return results;
            }
            catch(Exception exception)
            {
                var proxy = _proxy.current_proxy;
                while (proxy != null)
                {
                    _telegram.ChangeProxy(proxy);
                    if (_telegram.CheckConnection())
                    {
                        return -1;
                    }
                    proxy = _proxy.current_proxy;
                }
                throw new Exception("can't connect to telegram server. Proxy didn't help");
            }
        }

        private From GetRoomUser(int chat,int id)
        {
            try
            {
                var results = _telegram.GetChatMember(chat, id);
                return results;
            }
            catch (Exception exception)
            {
                var proxy = _proxy.current_proxy;
                while (proxy != null)
                {
                    _telegram.ChangeProxy(proxy);
                    if (_telegram.CheckConnection())
                    {
                        return null;
                    }
                    proxy = _proxy.current_proxy;
                }
                throw new Exception("can't connect to telegram server. Proxy didn't help");
            }
        }
    }
}
