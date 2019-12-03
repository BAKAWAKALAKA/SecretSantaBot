using System.Collections.Generic;

namespace SecretSantaBot
{
    public interface ICommandHandler
    {
        bool CanRespond(Message message);
        IEnumerable<Message> Respond(Message message);
    }
}