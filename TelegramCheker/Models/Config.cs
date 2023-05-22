using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCheker.Models;
    internal class Config
    {
    public long TargetChatId { get; set; } = 12345678;
    public string LoginPhoneNumber { get; set; } = "234234123423";
    public string? Password { get; set; }
    public long OutputChatId { get; set; }

    public Config(long targetChatId, string loginPhoneNumber, string? password, long outputChatId)
    {
        TargetChatId = targetChatId;
        LoginPhoneNumber = loginPhoneNumber;
        Password = password;
        OutputChatId = outputChatId;
    }

    public Config() : this (1234567, "YOUR_PHONE_HERE", "2FaPassword(if need)", 123434){}
}

