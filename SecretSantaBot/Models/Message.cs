using System.Collections.Generic;

namespace SecretSantaBot
{
    public class Message
    {
        public int msgId { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public string Room { get; set; }
        public string RoomType { get; set; }
        public string Command { get; set; }
        public string CommandId { get; set; }

        public bool show_alert { get; set; }
        public string callback_query_id { get; set; }

        public bool DisableNotification { get; set; }
        public int? reply_to_message_id { get; set; }
        public IEnumerable<string> SelectedUsers { get; set; } // если не выбран reply_to_message_id -> в текст записываются таргеты

        public Keyboard keybord { get; set; }
        public bool RemoveKeyboard { get; set; } // всем или в ответ на reply_to_message_id

        public bool ForceReplay { get; set; }
    }

    public class Keyboard
    {
        public List<List<Button>> Buttons { get; set; }
        public bool Inline { get; set; }
    }

    public class Button
    {
        public string Text { get; set; }
        public string Data { get; set; }
        public string URL { get; set; }
    }

    public enum RoomType
    {
        Private,
        Channel,
        Group,
        SuperGroup
    }
}