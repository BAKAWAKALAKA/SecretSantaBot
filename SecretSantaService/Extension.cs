using System;
using System.Collections.Generic;
using System.Text;
using SecretSantaBot;
using Telegram;
using Telegram.Models;

namespace SecretSantaService
{
    public static class Extension
    {
        public static SecretSantaBot.Message CreateBotMessage(this Telegram.Models.Message tlgMsg)
        {
            var mes = new SecretSantaBot.Message();
            mes.User = new User()
            {
                id = tlgMsg.from.id,
                name = $"{tlgMsg.from.username} {tlgMsg.from.first_name}" //todo исправить на чтото более удобное
            };
            mes.msgId = tlgMsg.message_id;
            mes.Room = tlgMsg.chat.id;
            mes.RoomType = tlgMsg.chat.type;
            mes.Text = tlgMsg.text;
            mes.reply_to_message_id = tlgMsg.reply_to_message?.message_id;
            return mes;
        }

        public static SecretSantaBot.Message CreateBotMessage(this Telegram.Models.CallbackQuery tlgMsg)
        {
            var mes = new SecretSantaBot.Message();
            mes.msgId = tlgMsg.message.message_id;
            mes.User = new User()
            {
                id = tlgMsg.from.id,
                name = $"{tlgMsg.from.username} {tlgMsg.from.first_name}"
            };
            mes.Room = tlgMsg.message.chat.id;
            mes.RoomType = tlgMsg.message.chat.type;
            mes.Text = tlgMsg.message.text;
            mes.reply_to_message_id = tlgMsg.message.reply_to_message?.message_id;
            mes.CommandId = tlgMsg.id;
            mes.Command = tlgMsg.data;
            return mes;
        }


        public static Telegram.Models.SendMessage CreateTelegramReplay(this SecretSantaBot.Message tlgMsg)
        {
            var mes = new Telegram.Models.SendMessage();
            mes.chat_id = tlgMsg.Room;
            mes.text = tlgMsg.Text;
            mes.disable_notification = tlgMsg.DisableNotification;
            mes.reply_to_message_id = tlgMsg.reply_to_message_id.GetValueOrDefault();
            if (tlgMsg.keybord != null && tlgMsg.keybord.Inline == false)
                mes.reply_markup = tlgMsg.keybord.CreateKeyboard();
            if (tlgMsg.keybord != null && tlgMsg.keybord.Inline == true)
                mes.reply_markup = tlgMsg.keybord.CreateInlineKeyboard();
            if (tlgMsg.RemoveKeyboard) mes.reply_markup = new ReplyKeyboardRemove();
            if (tlgMsg.ForceReplay) mes.reply_markup = new ForceReply();
            return mes;
        }

        public static Telegram.Models.answerCallbackQuery CreateTelegramCallbackQueryReplay(this SecretSantaBot.Message tlgMsg)
        {
            var mes = new Telegram.Models.answerCallbackQuery()
            {
                callback_query_id = tlgMsg.callback_query_id,
                show_alert = tlgMsg.show_alert,
                text = tlgMsg.Text
            };
            return mes;
        }

        public static Telegram.Models.InlineKeyboardMarkup CreateInlineKeyboard(this SecretSantaBot.Keyboard keyboard)
        {
            if (keyboard == null) return null;
            var tlgk = new InlineKeyboardMarkup() { inline_keyboard = new List<List<InlineKeyboardButton>>() };
            foreach (var list in keyboard.Buttons)
            {
                var tlList = new List<InlineKeyboardButton>();
                foreach (var k in list)
                {
                    tlList.Add(new InlineKeyboardButton()
                    {
                        text = k.Text,
                        callback_data = k.Data
                    });
                }
                tlgk.inline_keyboard.Add(tlList);
            }
            return tlgk;
        }
        public static Telegram.Models.KeybardReplyMarkup CreateKeyboard(this SecretSantaBot.Keyboard keyboard)
        {
            if (keyboard == null) return null;
            var tlgk = new KeybardReplyMarkup() { keyboard = new List<List<KeyButton>>() };
            foreach (var list in keyboard.Buttons)
            {
                var tlList = new List<KeyButton>();
                foreach (var k in list)
                {
                    tlList.Add(new KeyButton()
                    {
                        text = k.Text
                    });
                }
                tlgk.keyboard.Add(tlList);
            }
            return tlgk;
        }

    }
}
