using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramCheker.Models;
using TL;

namespace TelegramCheker.Controllers;
    internal class MainController
    {

    private WTelegram.Client client;
    private Data data;
    private CheckController checkController;
    private JLogger.Logger _logger;

    public MainController(WTelegram.Client Client, Data Data, JLogger.Logger logger)
    {
        client = Client;
        data = Data;
        checkController = new(data, logger);
        _logger = logger;
    }

    //новое сообщение в группе или канале
    public async void newMessageRecieved(UpdateNewMessage update, Dictionary<long, User> users, Dictionary<long, ChatBase> chats)
    {
        try
        {
            Console.WriteLine(update.message.Peer.ID.ToString());
            string username = "";
            //если это чат или канал
            if (chats.ContainsKey(update.message.Peer.ID))
            {
                //если это канал - цель
                if (update.message.Peer.ID == data.ProgramConfig.TargetChatId)
                {
                    username = getUsernameFromMessage(update.message.ToString());

                    checkController.recievedNewMessageFromTChat(update.message.ToString(), username, client);
                    Console.WriteLine("\n\n\n" + "\n\n" + username + "\n\n\n");
                    _logger.AddNewRecord($"Новое сообщение в целевом канале: {update.message} от {username}");
                }
            }
            else
            {
                var dialogs = await client.Messages_GetAllDialogs();

                if (dialogs.users[update.message.From.ID].IsActive)
                {
                    username = dialogs.users[update.message.From.ID].username;
                }
                checkController.recievedNewPersonalMessage(update.message.ToString(), username, client);
                _logger.AddNewRecord($"Новое личное сообщение: {update.message} от {username}");

            }
        }
        catch (Exception e) { Console.WriteLine(e.Message); _logger.AddNewErrorRecord(e.Message); }
    }

    // получить юзернейм из текста сообщения
    private string getUsernameFromMessage (string m)
    {
        char[] specChars = { '.', ',', ':', ';', ')', '(', '<', '>', '{', '}', '[', ']', '\\', '/', '!', '#', '%', '^', '&', ' ', '*', '-', '+' };
        string result = "-1";
        //разбиваем сообщение на подстроки
        string[] strings = m.Split('\n');

        Console.WriteLine("***************************");

        if (strings[0].Contains('@'))
        {
            //ищем индексы в первой подстроке
            var first = strings[0].IndexOf("(") + 1;
            var second = strings[0].IndexOf(")");

            result = m.Substring(first, second - first);
        }
        else
        {
            for(int i = 2; i < strings.Length; i++)
            {
                var tmp = strings[i].Split(specChars);
                foreach(var c in tmp)
                {
                    if (c.Contains('@'))
                    {
                        result = c;
                    }
                }
            }
        }


        return result;
    }//GetUsername
    
}

