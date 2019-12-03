using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot
{
    public class HelpCommandHandler : ICommandHandler
    {
        public bool CanRespond(Message message)
        {
            return (message.Command=="/help")? true: false;
        }

        public IEnumerable<Message> Respond(Message message)
        {
            var result = new List<Message>();

            result.Add(new Message());

            return result;
        }
    }
}
