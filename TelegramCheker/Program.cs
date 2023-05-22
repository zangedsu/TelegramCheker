// ручная конфигурация
static string Config(string what)
{
    switch (what)
    {
        case "api_id": return "22773979";
        case "api_hash": return "d3dcbbe6beaeb05dbbc4798db8e10d59";
        case "phone_number": return "+79770174795";
        case "verification_code": Console.Write("Код подтверждения(отправлен в СМС или Push): "); return Console.ReadLine();
        case "first_name": return "John";      // Если номер ещё не зарегистрирован
        case "last_name": return "Doe";        // Если номер ещё не зарегистрирован
        case "password": return "1944Qwerty!";     // Пароль для 2fa
        default: return null;                  // Конфиг по умолчанию
    }
}

//Экземпляр класса клиента
using var client = new WTelegram.Client(Config);
//Запуск интерактивной настройки
var myself = await client.LoginUserIfNeeded();
Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");



// пример использования
// получаем список личных диалогов
var dialogs = await client.Messages_GetAllDialogs();
// получаем список чатов и каналов
var chats = await client.Messages_GetAllChats();

//выводим список личных диалогов
Console.WriteLine("Список личных диалогов:");
foreach (var (id, chat) in dialogs.users)
    //if (chat.IsActive)
        Console.WriteLine($"{id,10}: {chat}");

// выводим список чатов и каналов
Console.WriteLine("Список каналов и чатов:");
foreach (var (id, chat) in chats.chats)
    if (chat.IsActive)
        Console.WriteLine($"{id,10}: {chat}");

// отправка сообщения в чат или канал
//Console.Write("Type a chat ID to send a message: ");
//long chatId = long.Parse(Console.ReadLine());
//var target = chats.chats[chatId];
//Console.WriteLine($"Sending a message in chat {chatId}: {target.Title}");
//await client.SendMessageAsync(target, "Hello, World");

// отправка личного сообщения
Console.Write("Type a chat ID to send a message: ");
long chatId = long.Parse(Console.ReadLine());
var target = dialogs.users[chatId];
Console.WriteLine($"Sending a message in chat {chatId}: {target.MainUsername}");
await client.SendMessageAsync(target, "Hello, World");