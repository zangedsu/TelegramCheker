using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCheker.Models;

    internal class Subject
    {
    // TODO: Реализовать модель, описывающую проверяемого пользователя
    public long UserId { get; set; }
    public bool IsTestsPassed { get; set; }
    public bool IsOnCheckNow { get; set; }
    public DateTime FirstMessageSentAt { get; set; }

    /// <summary>
    /// Класс, описывающий проверяемого пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="isTestPassed"></param>
    /// <param name="isOnCheckNow"></param>
    /// <param name="firstMessageSentAt"></param>
    public Subject(long userId, bool isTestPassed, bool isOnCheckNow, DateTime firstMessageSentAt)
    {
        UserId = userId;
        IsTestsPassed = isTestPassed;
        IsOnCheckNow = isOnCheckNow;
        FirstMessageSentAt = firstMessageSentAt;
    }
}

