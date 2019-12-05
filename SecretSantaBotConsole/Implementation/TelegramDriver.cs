using SecretSantaBot;
using Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSantaBotConsole.Implementation
{
    public class TelegramDriver : IDriver
    {
        private Telegram.Telegram _telegram;
        private int _lastTime; // maybe be using for getupdate method
        public TelegramDriver(Telegram.Telegram telegram, ProxyParser.ProxyParser proxy)
        {

        }

        public IEnumerable<Message> GetUpdates()
        {
            var results = _telegram.GetUpdates();
            var messages = results.Select(q=>q.message.CreateBotMessage());
            throw new NotImplementedException();
            return messages;
        }

        public void SendResponse(Message response)
        {
            var message = response.CreateTelegramReplay();
            throw new NotImplementedException();
            _telegram.SendMessage(message);
        }

        private int GetRoomUserCount()
        {
            throw new NotImplementedException();
        }

        private void GetRoomUser()
        {
            throw new NotImplementedException();
        }
    }
}
