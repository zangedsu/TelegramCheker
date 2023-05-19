// ручная конфигурация
static string Config(string what)
{
    switch (what)
    {
        case "api_id": return "22773979";
        case "api_hash": return "d3dcbbe6beaeb05dbbc4798db8e10d59";
        case "phone_number": return "+380713350539";
        case "verification_code": Console.Write("Code: "); return Console.ReadLine();
        case "first_name": return "John";      // Если номер ещё не зарегистрирован
        case "last_name": return "Doe";        // Если номер ещё не зарегистрирован
        case "password": return "secret!";     // Пароль для 2fa
        default: return null;                  // Конфиг по умолчанию
    }
}

//Экземпляр класса клиента
using var client = new WTelegram.Client(Config);
//Запуск интерактивной настройки
var myself = await client.LoginUserIfNeeded();
Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");



// пример использования
var chats = await client.Messages_GetAllChats();
Console.WriteLine("This user has joined the following:");
foreach (var (id, chat) in chats.chats)
    if (chat.IsActive)
        Console.WriteLine($"{id,10}: {chat}");
Console.Write("Type a chat ID to send a message: ");
long chatId = long.Parse(Console.ReadLine());
var target = chats.chats[chatId];
Console.WriteLine($"Sending a message in chat {chatId}: {target.Title}");
await client.SendMessageAsync(target, "Hello, World");