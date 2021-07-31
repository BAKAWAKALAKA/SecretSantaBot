using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using SecretSantaBot;
using SecretSantaBotConsole.Implementation;

namespace SecretSantaBotConsole
{
    class Program
    {
        // it can be deamon or process but I like it this way
        // I do it just for my friends afterall
        static void Main(string[] args)
        {
            Console.WriteLine("Bot start");
            var telegram = new Telegram.Telegram();
            SecretSantaBot.Extension.TelegramDriver = new TelegramDriver(telegram);
            var bot = new SecretSantaBot.SecretSantaBot(new TelegramDriver(telegram), new ICommandHandler[] { new SecretCommandHandler(), new PrivateRoomCommandHandler() });
            bot.Run(new TimeSpan(0,0,5));
            while (true) { }
        }
    }
}
