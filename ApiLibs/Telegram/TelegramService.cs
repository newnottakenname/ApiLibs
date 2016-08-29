﻿using ApiLibs.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibs.Telegram
{
    public class TelegramService : Service
    {
        private List<From> contacts;

        public event MessageHandler MessageRecieved;
        public delegate void MessageHandler(Message m, EventArgs e);
        public Memory mem;

        public string Telegram_token;

        public TelegramService(string token, string applicationDirectory)
        {
            Telegram_token = token;
            SetUp("https://api.telegram.org/bot" + Telegram_token);
            mem = new Memory(applicationDirectory);
            ReadStoredUsernames();
        }

        //TODO
        public async Task GetMe()
        {
            await HandleRequest("/getMe", Call.POST, new List<Param>());
        }

        public async Task SendMessage(string username, string message, ParseMode mode = ParseMode.None, bool webPreview = true, int reply_to_message_id = -1)
        {
            await SendMessage(ConvertFromUsernameToID(username), message, mode, webPreview, reply_to_message_id);
        }
        
        public async Task SendMessage(int id, string message, ParseMode mode = ParseMode.None, bool webPreview = true, int reply_to_message_id = -1)
        {
            List<Param> param = new List<Param>
            {
                new Param("chat_id", id.ToString()),
                new Param("text", message),
                new Param("disable_web_page_preview", (!webPreview).ToString()),
            };
            switch (mode)
            {
                case ParseMode.HTML:
                    param.Add(new Param("parse_mode","HTML"));
                    break;
                case ParseMode.Markdown:
                    param.Add(new Param("parse_mode", "Markdown"));
                    break;
            }

            if (reply_to_message_id != -1)
            {
                param.Add(new Param("reply_to_message_id", reply_to_message_id.ToString()));
            }

            await HandleRequest("/sendMessage", Call.GET, param);
        }

        public int ConvertFromUsernameToID(string userid)
        {
            foreach(From contact in contacts)
            {
                if(contact.username == userid)
                {
                    return contact.id;
                }
            }
            throw new KeyNotFoundException(userid + "was not found. Did you type it correctly?");
        }

        public void LookForMessages()
        {
            Thread t = new Thread(async () =>
            {
                while (true)
                {
                    List<Message> mList = await WaitForNextMessage();
                    foreach (Message m in mList)
                    {
                        MessageRecieved.Invoke(m, EventArgs.Empty);
                    }
                }
            });
            t.Start();
        }

        private async Task<List<Message>> WaitForNextMessage()
        {
            int updateId = (mem.ReadFile<Result>("data/telegram/lastID")).update_id;

            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", parameters: new List <Param> { new Param("timeout", "100"), new Param("offset", updateId.ToString()) });
            foreach (Result message in messages.result)
            {
                AddFrom(message.message.from);
            }

            List<Message> result = new List<Message>();

            messages.result.Reverse();
            foreach (Result message in messages.result)
            {
                if (message.update_id == updateId)
                {
                    break;
                }
                result.Add(message.message);
            }

            if (result.Count != 0)
            {
                mem.WriteFile("data/telegram/lastID", messages.result[0]);
            }

            return result;
        }

        public async Task<List<Message>> GetMessages()
        {
            int updateId = mem.ReadFile<Result>("data/telegram/lastID").update_id;
            List<Param> parameters = new List<Param>();
            if (updateId != -1)
            {
                parameters.Add(new Param("offset", updateId.ToString()));
            }
            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", parameters: parameters);
            foreach(Result message in messages.result)
            {
                AddFrom(message.message.from);
            }

            List<Message> result = new List<Message>();

            messages.result.Reverse();

            foreach (Result message in messages.result)
            {
                if (message.update_id == updateId)
                {
                    break;
                }
                result.Add(message.message);
            }

            if (result.Count != 0)
            {
                mem.WriteFile("data/telegram/lastID", messages.result[0]);
            }

            return result;
        }

        private void AddFrom(From from)
        {
            if (!contacts.Contains(from))
            {
                contacts.Add(from);
            }
            WriteUsernames();
        }

        private void WriteUsernames()
        {
            mem.WriteFile("data/telegram/usernames", contacts);
        }

        private void ReadStoredUsernames()
        {
            contacts = mem.ReadFile<List<From>>("data/telegram/usernames");
        }

        
    }

    public enum ParseMode
    {
        None, Markdown, HTML
    }
}
