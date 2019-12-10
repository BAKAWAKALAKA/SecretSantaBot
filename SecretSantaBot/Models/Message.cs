using System.Collections.Generic;

namespace SecretSantaBot
{
    public class Message
    {
        public string Text { get; set; }
        public User User { get; set; }
        public int Room { get; set; }
        public string Command { get; set; }
        public int UserCount { get; set; }
        public IEnumerable<string> SelectedUsers { get; set; }
    }
}