using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCheker.Models;
    internal class SpamBotIndicators
    {
    //TODO: Реализовать модель, описывающую признаки спам-ботов и нежелательных сообщений
    public List<string> Phrases { get; set; }
    public int ReplyDelayInSeconds { get; set; }

    public SpamBotIndicators(List<string> phrases, int replyDelayInSeconds) 
    { 
        Phrases = phrases;
        ReplyDelayInSeconds = replyDelayInSeconds;
    }

    public SpamBotIndicators() : this (new(), 1){ }
}

