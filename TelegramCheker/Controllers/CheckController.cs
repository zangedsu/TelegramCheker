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


    public CheckController(Data data) 
        {
        _data = data; 
        }

    public async void recievedNewMessageFromTChat(string message, string username, WTelegram.Client client)
    {
      username = username.Trim('@');
        int index = -1;

        for (int i = 0; i < _data.Subjects.Count; i++)
        {
            if (_data.Subjects[i].UserName == username)
            {
                //если пользователь уже есть в базе
                // TODO: произвести обработку (проверку)
                index = i; break;
            }
        }

        if(index == -1)
        {
            Thread.Sleep(1000);
            _data.Subjects.Add(new(username));
            _data.SerializeSubjectsData();
            sendFirstMessage(username, client);
        }

    }

    //получено личное сообщение
    public async void recievedNewPersonalMessage(string message, string username, WTelegram.Client client)
    {
        username = username.Trim('@');
        int index = -1;

        for (int i = 0; i < _data.Subjects.Count; i++)
        {
            if (_data.Subjects[i].UserName == username)
            {
                //если пользователь уже есть в базе
                // TODO: произвести обработку (проверку)
                index = i; break;
            }
        }//for

        if(index != -1)
        {
            Console.WriteLine($"\nПользователь {_data.Subjects[index].UserName} из списка проверки написал: {message}\n");
            // TODO: реализовать проверку ответа пользователя

            bool spamFlag = false;

            foreach (string phrase in _data.Indicators.Phrases)
            {
                if (message.ToLower().Contains(phrase.ToLower())) 
                { spamFlag = true; 
                Console.WriteLine($"\n@{username} - не прошел проверку по одному из маркеров");
                    _data.Subjects[index].IsTestsPassed = false;
                    _data.Subjects[index].IsOnCheckNow = false;
                    _data.SerializeSubjectsData();

                    break;
                }
            }//foreach
            if (!spamFlag) { _data.Subjects[index].IsTestsPassed = true; _data.Subjects[index].IsOnCheckNow = false; _data.SerializeSubjectsData();
                sendResultMessageToAdminChat(username, message, client); }
        }
    }//recieved personal m

    // отправка первого сообщеня
    private async void sendFirstMessage(string username, WTelegram.Client client)
     {
        var resolved = await client.Contacts_ResolveUsername(username); // username without the @
        await client.SendMessageAsync(resolved, _data.ProgramConfig.FirstMessage);
        Console.WriteLine("Отправили первое сообщение в чат с " + "@" + username);
     }

    // отправить сообщение в админский чат с результатом

    private async void sendResultMessageToAdminChat(string username,string message, WTelegram.Client client)
    {
        var chats = await client.Messages_GetAllChats();
        InputPeer inputpeer = chats.chats[_data.ProgramConfig.OutputChatId];

        await client.SendMessageAsync(inputpeer, $"Пользователь @{username} успешно прошел проверку.\nТекст ответа пользователя на моё сообщение:\n" +
            $"\n{message}") ;
        Console.WriteLine("\nОтправили сообщение в админский чат");
    }

    //сериализация данных


    }

