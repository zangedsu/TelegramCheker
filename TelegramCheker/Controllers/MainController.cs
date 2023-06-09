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
    private int _attemptsCounter = 0;

    public MainController(WTelegram.Client Client, Data Data, JLogger.Logger logger)
    {
        client = Client;
        data = Data;
        checkController = new(data, logger);
        _logger = logger;
    }
    // TODO: Исправить костыльную систему с попытками при неактуальном словаре
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
                   // username = getUsernameFromMessage(update.message.ToString());
                   if(TryGetUsernameFromMessage(update.message.ToString(), out username))
                    {
                        checkController.recievedNewMessageFromTChat(update.message.ToString(), username, client);
                        Console.WriteLine("\n\n\n" + "\n\n" + username + "\n\n\n");
                    }
                    else
                    {
                        Console.WriteLine("\nНе удалось распознать юзернейм в сообщении\n");
                        _logger.AddNewErrorRecord("Не удалось распознать юзернейм в сообщении");
                    }
                    _logger.AddNewRecord($"Новое сообщение в целевом канале: {update.message} от {username}");
                }
            }
            else
            {
                var dialogs = await client.Messages_GetAllDialogs();
                long? userId;

                userId = update.message.From?.ID != null ? update.message.From?.ID : update.message.Peer?.ID;

                if(userId != null)
                {
                    if (dialogs.users.ContainsKey((long)userId))
                    {
                        if (dialogs.users[(long)userId].IsActive)
                        {
                            username = dialogs.users[(long)userId].username;
                        }
                        checkController.recievedNewPersonalMessage(update.message.ToString(), username, client);
                        _logger.AddNewRecord($"Новое личное сообщение: {update.message} от {username}");
                    }//if contains
                    else
                    {
                        if(_attemptsCounter >= 3)
                        {
                            _attemptsCounter++;
                            Console.WriteLine($"Не удалось обработать сообщение, попытка {_attemptsCounter} из 3");
                            _logger.AddNewErrorRecord($"Не удалось обработать сообщение, попытка {_attemptsCounter} из 3");
                            Thread.Sleep(3000);
                            newMessageRecieved(update, users, chats);
                        }
                    }
                }//if not null
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            _logger.AddNewErrorRecord(e.Message);

        }
    }//recievedNewMessage

    // получить юзернейм из текста сообщения
    private string getUsernameFromMessage(string m)
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
            for (int i = 2; i < strings.Length; i++)
            {
                var tmp = strings[i].Split(specChars);
                foreach (var c in tmp)
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

    // попробовать получить юзернейм из текста сообщения
    private bool TryGetUsernameFromMessage(string m, out string result)
    {
        char[] specChars = { '.', ',', ':', ';', ')', '(', '<', '>', '{', '}', '[', ']', '\\', '/', '!', '#', '%', '^', '&', ' ', '*', '-', '+' };
         result = "-1";
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
            for (int i = 2; i < strings.Length; i++)
            {
                var tmp = strings[i].Split(specChars);
                foreach (var c in tmp)
                {
                    if (c.Contains('@'))
                    {
                        result = c;
                    }
                }
            }
        }
        return result != "-1" ? true : false ;
    }//tryGetUsername


}

