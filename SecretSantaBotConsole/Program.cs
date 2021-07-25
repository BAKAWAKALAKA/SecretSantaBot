using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using SecretSantaBot;

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
            SecretSantaBot.Extension.TelegramDriver = new SecretSantaBotConsole.Implementation.TelegramDriver(telegram);
            var privatHandler = new PrivateRoomCommandHandler();
            var bot = new SecretSantaBot.SecretSantaBot(new SecretSantaBotConsole.Implementation.TelegramDriver(telegram), new ICommandHandler[] { new SecretCommandHandler(), privatHandler });
            bot.Run(new TimeSpan(0,0,5));
            while (true) { }
        }
    }
}
