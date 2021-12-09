using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SecretSantaBot
{
    public class SecretSantaBot
    {
        Timer _timer;
        IDriver _driver;
        ICommandHandler[] _handlers;
        ILogger _logger;

        public SecretSantaBot(IDriver driver, ICommandHandler[] handlers, ILogger logger = null)
        {
            _logger = logger;
            _driver = driver;
            Extension.TelegramDriver = _driver;
            _handlers = handlers;
            _timer = new Timer((x) => this.Update());
        }


        public void Run(TimeSpan period)
        {
            _timer.Change(new TimeSpan(), period);
        }

        /// <summary>
        /// such simple realization because we don't need to use notify, deffered messages or hold callback message
        /// </summary>
        public void Update()
        {
            try
            {
                var updates = _driver.GetUpdates();
                var result = new List<Message>();
                foreach (var update in updates)
                {
                    foreach (var handler in _handlers)
                    {
                        if (handler.CanRespond(update))
                        {
                            var responses = handler.Respond(update);
                            if (responses.Any())
                            {
                                result.AddRange(responses);
                            }
                        }
                    }
                }
                _logger.LogDebug($"response count: {result.Count}");
                foreach (var response in result)
                {
                    try
                    {
                        _driver.SendResponse(response);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e,e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
            }
        }
    }
}
