using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCheker.Controllers;
    internal class MainController
    {
    public void newMessageRecieved(string message, long userId) 
    { 
        Console.WriteLine($"Получено сообщение: {message}\nОт: {userId}");
    }

    }

