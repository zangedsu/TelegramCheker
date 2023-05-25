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
            Console.WriteLine($"ЭТО ЧАТ!!!!\n##########\n{update.message}\n########\n{update.message.Peer.ID}");
        }
        else if(users.ContainsKey(update.message.Peer.ID))
        {
            Console.WriteLine($"ЭТО ЛИЧНОЕ СООБЩЕНИЕ!!!!\n##########\n{update.message}\n########\n{update.message.Peer.ID}");
        }
        //  Console.WriteLine($"Получено сообщение: {update.message}\nВ группе с ID : {123}");
    }
}

