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

        public SecretSantaBot(IDriver driver, ICommandHandler[] handlers)
        {
            _driver = driver;
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
        private void Update()
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
            foreach (var response in result)
            {
                _driver.SendResponse(response);
            }
        }
    }
}
