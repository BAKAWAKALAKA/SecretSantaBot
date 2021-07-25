using System.Collections.Generic;

namespace SecretSantaBot
{
    public interface IDriver
    {
        IEnumerable<Message> GetUpdates();
        bool SendResponse(Message response);

        int GetRoomUserCount(string chatId);

        User GetUser(int id, string chatId);
    }
}