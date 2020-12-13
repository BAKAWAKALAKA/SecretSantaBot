using System;
using SecretSantaBot;

namespace SecretSantaBotConsole
{
    class Program
    {
        // it can be deamon or process but I like it this way
        // I do it just for my friends afterall
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var telegram = new Telegram.Telegram();
            var helpH = new HelpCommandHandler();
            var se = new SecretCommandHandler();
            var bot = new SecretSantaBot.SecretSantaBot(new SecretSantaBotConsole.Implementation.TelegramDriver(telegram), new ICommandHandler[] { se, helpH });
            bot.Run(new TimeSpan(0,0,2));
            while (true) { }
        }
    }
}
