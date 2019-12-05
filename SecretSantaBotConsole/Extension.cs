using System;
using System.Collections.Generic;
using System.Text;
using SecretSantaBot;
using Telegram;
using Telegram.Models;

namespace SecretSantaBotConsole
{
    public static class Extension
    {
        public static SecretSantaBot.Message CreateBotMessage(this Telegram.Models.Message tlgMsg)
        {
            var mes = new SecretSantaBot.Message();
            //some staff
            return mes;
        }

        public static Telegram.Models.Message CreateTelegramMessage(this SecretSantaBot.Message tlgMsg)
        {
            var mes = new Telegram.Models.Message();
            //some staff
            return mes;
        }

        public static Telegram.Models.SendMessage CreateTelegramReplay(this SecretSantaBot.Message tlgMsg)
        {
            var mes = new Telegram.Models.SendMessage();
            //some staff
            return mes;
        }
    }
}
