using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;

namespace TelegramCheker.Controllers;
    internal class MainController
    {

    //новое сообщение в группе или канале
    public void newMessageRecieved(UpdateNewMessage update)
    {
        update.message.From
        Console.WriteLine($"Получено сообщение: {update.message}\nВ группе с ID : {123}");
    }
}

