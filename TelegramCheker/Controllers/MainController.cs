using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;

namespace TelegramCheker.Controllers;
    internal class MainController
    {


    public MainController()
    {
        
    }

    //новое сообщение в группе или канале
    public void newMessageRecieved(UpdateNewMessage update, Dictionary<long, User> users, Dictionary<long, ChatBase> chats)
    {
        //если это чат или канал
        if (chats.ContainsKey(update.message.Peer.ID))
        {
            if(update.message.Peer.ID == 1880944338)
            {
                string username = getUsernameFromMessage( update.message.ToString());
                   

                Console.WriteLine("\n\n\n"+ "\n\n" + username + "\n\n\n");
            }
            Console.WriteLine($"ЭТО ЧАТ!!!!\n##########\n{update.message}\n########\n{update.message.Peer.ID}");
        }
        else if(users.ContainsKey(update.message.Peer.ID))
        {
            Console.WriteLine($"ЭТО ЛИЧНОЕ СООБЩЕНИЕ!!!!\n##########\n{update.message}\n########\n{update.message.Peer.ID}");
        }
        //  Console.WriteLine($"Получено сообщение: {update.message}\nВ группе с ID : {123}");
    }

    private string getUsernameFromMessage (string m)
    {
        char[] specChars = { '.', ',', ':', ';', ')', '(', '<', '>', '{', '}', '[', ']', '\\', '/', '!', '#', '%', '^', '&' };
        string result = "";
        //разбиваем сообщение на подстроки
        string[] strings = m.Split('\n');

        Console.WriteLine("***************************");
       // foreach (string s in strings) { Console.WriteLine(s + "\n"); }
       // Console.WriteLine("***************************");

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
    }
    
}

