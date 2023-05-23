using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCheker.Models;
    internal class Config
    {
    public long TargetChatId { get; set; }
    public string LoginPhoneNumber { get; set; }
    public string? Password { get; set; }
    public long OutputChatId { get; set; }
    public string FirstMessage { get; set; }

    public Config(long targetChatId, string loginPhoneNumber, string? password, long outputChatId, string firstMessage)
    {
        TargetChatId = targetChatId;
        LoginPhoneNumber = loginPhoneNumber;
        Password = password;
        OutputChatId = outputChatId;
        FirstMessage = firstMessage;
    }

    public Config() : this (1234567, "YOUR_PHONE_HERE", "2FaPassword(if need)", 123434, 
        "Здравствуйте. Вы искали специалиста по контекстной рекламе. Подскажите, еще данный вопрос актуален?") {}
}

