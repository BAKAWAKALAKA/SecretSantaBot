using System.Collections.Generic;

namespace SecretSantaBot
{
    public interface IDriver
    {
        IEnumerable<Message> GetUpdates();
        void SendResponse(Message response);
    }
}