using SecretSantaBot;
using Telegram;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBotConsole.Implementation
{
    public class TelegramDriver : IDriver
    {
        private Telegram.Telegram _telegram;

        public TelegramDriver(Telegram.Telegram telegram, ProxyParser.ProxyParser proxy)
        {

        }

        public IEnumerable<Message> GetUpdates()
        {
            throw new NotImplementedException();
        }

        public void SendResponse(Message response)
        {
            throw new NotImplementedException();
        }

        private void GetRoomUserCount()
        {

        }
    }
}
