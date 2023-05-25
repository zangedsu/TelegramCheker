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
        var resolved = await client.Contacts_ResolveUsername("FDV_photo"); // username without the @
        await client.SendMessageAsync(resolved, _data.ProgramConfig.FirstMessage);

       // var tmp = from s in _subjects
       //           from n in s.UserName

    }
  
    }

