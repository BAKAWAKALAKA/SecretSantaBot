using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void Update()
        {
            var updates = _driver.GetUpdates();
            var responses = new List<Message>();
            foreach (var update in updates)
            {
                foreach (var handler in _handlers)
                {
                    if (handler.CanRespond(update))
                    {
                        var response = handler.Respond(update);
                        if (response.Any())
                        {
                            responses.AddRange(response);
                        }
                    }
                }
            }
            foreach (var response in responses)
            {
                _driver.SendResponse(response);
            }
        }

    }
}
