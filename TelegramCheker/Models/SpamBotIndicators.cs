﻿using System;
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

    /// <summary>
    /// Класс, описывающий индикаторы спама\ботности
    /// </summary>
    /// <param name="phrases">List<string> с фразами</param>
    /// <param name="replyDelayInSeconds">int с минимальной задержкой в секундах</param>
    public SpamBotIndicators(List<string> phrases, int replyDelayInSeconds) 
    { 
        Phrases = phrases;
        ReplyDelayInSeconds = replyDelayInSeconds;
    }

    public SpamBotIndicators() : this(new() {"Тут нужно написать признаки ботности", "Признак 1","Признак 2", "И Т. Д."}, 1){ }


}

