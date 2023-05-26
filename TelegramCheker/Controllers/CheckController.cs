using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramCheker.Models;
using TL;

namespace TelegramCheker.Controllers;
    internal class CheckController
    {
    private Data _data;
    private List<Subject> _subjects;

    public CheckController(Data data) 
        {
        _data = data; 
        _subjects = new List<Subject>();
        }

    public async void recievedNewMessageFromTChat(string message, string username, WTelegram.Client client)
    {
      username = username.Trim('@');
        int index = -1;

        for (int i = 0; i < _subjects.Count; i++)
        {
            if (_subjects[i].UserName == username)
            {
                //если пользователь уже есть в базе
                // TODO: произвести обработку (проверку)
                index = i; break;
            }
        }

        if(index == -1)
        {
            Thread.Sleep(1000);
            _subjects.Add(new(username));
            sendFirstMessage(username, client);
        }

    }

    //получено личное сообщение
    public async void recievedNewPersonalMessage(string message, string username, WTelegram.Client client)
    {
        username = username.Trim('@');
        int index = -1;

        for (int i = 0; i < _subjects.Count; i++)
        {
            if (_subjects[i].UserName == username)
            {
                //если пользователь уже есть в базе
                // TODO: произвести обработку (проверку)
                index = i; break;
            }
        }//for

        if(index != -1)
        {
            Console.WriteLine($"\nПользователь {_subjects[index].UserName} из списка проверки написал!\n");
            // TODO: реализовать проверку ответа пользователя

            bool spamFlag = false;

            foreach (string phrase in _data.Indicators.Phrases)
            {
                if (message.ToLower().Contains(phrase.ToLower())) { spamFlag = true; break;}
            }
        }
    }

    // отправка первого сообщеня
    private async void sendFirstMessage(string username, WTelegram.Client client)
     {
        var resolved = await client.Contacts_ResolveUsername(username); // username without the @
        await client.SendMessageAsync(resolved, _data.ProgramConfig.FirstMessage);
        Console.WriteLine("Отправили первое сообщение в чат с " + "@" + username);
     }

    // проверка ответа пользователя

  
    }

