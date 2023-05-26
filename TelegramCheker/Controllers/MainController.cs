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

    public MainController(WTelegram.Client Client, Data Data)
    {
        client = Client;
        data = Data;
        checkController = new(data);
    }

    //новое сообщение в группе или канале
    public async void newMessageRecieved(UpdateNewMessage update, Dictionary<long, User> users, Dictionary<long, ChatBase> chats)
    {
        //если это чат или канал
        if (chats.ContainsKey(update.message.Peer.ID))
        {
            //если это канал - цель
            if(update.message.Peer.ID == data.ProgramConfig.TargetChatId)
            {
                string username = getUsernameFromMessage( update.message.ToString());

               checkController.recievedNewMessageFromTChat(update.message.ToString(), username, client);
                Console.WriteLine("\n\n\n"+ "\n\n" + username + "\n\n\n");
            }
        }
        else if(users.ContainsKey(update.message.Peer.ID))
        {
           checkController.recievedNewPersonalMessage(update.message.ToString(), users[update.message.Peer.ID].username, client);
        }

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

