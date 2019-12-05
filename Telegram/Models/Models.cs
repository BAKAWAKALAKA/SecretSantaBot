using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Models
{
    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string language_code { get; set; }
    }

    public class Chat
    {
        public int id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool all_members_are_administrators { get; set; }
    }

    public class Contact
    {
        public string phone_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int user_id { get; set; }
    }

    public class Photo
    {
        public string file_id { get; set; }
        public int file_size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Thumb
    {
        public string file_id { get; set; }
        public int file_size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Sticker
    {
        public int width { get; set; }
        public int height { get; set; }
        public string emoji { get; set; }
        public string set_name { get; set; }
        public Thumb thumb { get; set; }
        public string file_id { get; set; }
        public int file_size { get; set; }
    }



    public class KeyButton
    {
        public string text { get; set; }
        public bool request_contact { get; set; } //Опционально Если значение True, то при нажатии на кнопку боту отправится контакт пользователя с его номером телефона. Доступно только в диалогах с ботом.
        public bool request_location { get; set; } //Опционально. Если значение True, то при нажатии на кнопку боту отправится местоположение пользователя. Доступно только в диалогах с ботом.
    }

    public interface ReplyMarkup
    {

    }

    public class KeybardReplyMarkup: ReplyMarkup
    {
        public bool hide_keyboard { get; set; } // if it true then don't use a keyboard
        public bool resize_keyboard { get; set; }
        public bool one_time_keyboard { get; set; }
        public bool selective { get; set; } //Опционально Этот параметр нужен, чтобы показывать клавиатуру только определённым пользователям. Цели: 1) пользователи, которые были @упомянуты в поле text объекта Message; 2) если сообщения бота является ответом (содержит поле reply_to_message_id), авторы этого сообщения.
        public List<List<KeyButton>> keyboard { get; set; }
    }

    public class InlineKeyboardButton
    {
        public string text { get; set; }
        public string url { get; set; } //Опционально. URL, который откроется при нажатии на кнопку
        public string callback_data { get; set; }//Опционально. Данные, которые будут отправлены в callback_query при нажатии на кнопку
    }

    public class InlineKeyboardMarkup: ReplyMarkup
    {
        public List<List<InlineKeyboardButton>> inline_keyboard { get; set; }
    }

    //todo editMessageText

    //todo split Message and Send Message in one (by interface or base class) because I don't like where they aren't alike
    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public From new_chat_participant { get; set; }
        public From left_chat_member { get; set; }
        public string new_chat_title { get; set; }
        public From new_chat_member { get; set; }
        public List<From> new_chat_members { get; set; }
        public ReplyMarkup reply_markup { get; set; }
        public Message reply_to_message { get; set; }
        public Contact contact { get; set; }
        public Photo photo { get; set; }
        public Sticker sticker { get; set; }
        public From forward_from { get; set; }
        public int forward_date { get; set; }
        public string caption { get; set; }
    }

    public class SendMessage
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public string parse_mode { get; set; }
        public bool disable_web_page_preview { get; set; }
        public ReplyMarkup reply_markup { get; set; }
        public bool disable_notification { get; set; }
        public int reply_to_message_id { get; set; }
    }

    public class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }

    public class Response
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }
    }
}
