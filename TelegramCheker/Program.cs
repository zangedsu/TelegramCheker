// ручная конфигурация
using JLogger;
using TelegramCheker.Controllers;
using TelegramCheker.Models;
using TL;
using WTelegram;

var loger = new JLogger.Logger();
Console.WriteLine("\n\n#########\nTChecker v0.9.4\n##########\nПо все вопросам писать в тг @FDV_photo");
loger.AddNewRecord("Test");
Thread.Sleep(5000);

Data config = new Data();
 string Config(string what)
{
    switch (what)
    {
        case "api_id": return "22773979";
        case "api_hash": return "d3dcbbe6beaeb05dbbc4798db8e10d59";
        case "phone_number": return config.ProgramConfig.LoginPhoneNumber;
        case "verification_code": Console.Write("Код подтверждения(отправлен в СМС или Push): "); return Console.ReadLine();
        case "first_name": return "John";      // Если номер ещё не зарегистрирован
        case "last_name": return "Doe";        // Если номер ещё не зарегистрирован
        case "password": return "!";     // Пароль для 2fa
        case "session_pathname": return @"./Data/TChecker.session";     //Путь к файлу сессии
        default: return null;                  // Конфиг по умолчанию
    }
}

try
{
    //Экземпляр класса клиента
    using var client = new WTelegram.Client(Config);
    //Запуск интерактивной настройки
    var myself = await client.LoginUserIfNeeded();
    Console.WriteLine($"Вошли в аккаунт {myself} (id {myself.id})");

    //словари для хранения диалогов
    Dictionary<long, User> Users = new();
    Dictionary<long, ChatBase> Chats = new();


    MainController controller = new MainController(client, config, loger);
    // подписываемся на событие
    client.OnUpdate += Client_OnUpdate;




    // пример использования 

    // получаем список личных диалогов
    var dialogs = await client.Messages_GetAllDialogs();
    // получаем список чатов и каналов
    //var chats = await client.Messages_GetAllChats();

    dialogs.CollectUsersChats(Users, Chats);

    //выводим список личных диалогов
    Console.WriteLine("Список личных диалогов:");
    foreach (var (id, chat) in dialogs.users)
        //if (chat.IsActive)
        Console.WriteLine($"{id,10}: {chat}");

    // выводим список чатов и каналов
    //Console.WriteLine("Список каналов и чатов:");
    //foreach (var (id, chat) in chats.chats)
    //   if (chat.IsActive)
    //      Console.WriteLine($"{id,10}: {chat}");
    //
    Console.ReadKey();
    // отправка сообщения в чат или канал
    //Console.Write("Type a chat ID to send a message: ");
    //long chatId = long.Parse(Console.ReadLine());
    //var target = chats.chats[chatId];
    //Console.WriteLine($"Sending a message in chat {chatId}: {target.Title}");
    //await client.SendMessageAsync(target, "Hello, World");

    // отправка личного сообщения
    //Console.Write("Type a chat ID to send a message: ");
    //long chatId = long.Parse(Console.ReadLine());
    //var target = dialogs.users[chatId];
    //Console.WriteLine($"Sending a message in chat {chatId}: {target.MainUsername}");
    //await client.SendMessageAsync(target, "Hello, World");



    // апдейты
    async Task Client_OnUpdate(UpdatesBase updates)
    {
        updates.CollectUsersChats(Users, Chats);
        foreach (var update in updates.UpdateList)
            switch (update)
            {
                case UpdateNewMessage unm:
                    controller.newMessageRecieved(unm, Users, Chats);
                    await DisplayMessage(unm.message);
                    loger.AddNewRecord($"Получено сообщение: {unm.message}"); break;
                case UpdateEditMessage uem: await DisplayMessage(uem.message, true); break;
                // Note: UpdateNewChannelMessage and UpdateEditChannelMessage are also handled by above cases
                case UpdateDeleteChannelMessages udcm: Console.WriteLine($"{udcm.messages.Length} сообщения были удалены из {Chat(udcm.channel_id)}"); break;
                case UpdateDeleteMessages udm: Console.WriteLine($"{udm.messages.Length} message(s) deleted"); break;
                case UpdateUserTyping uut: Console.WriteLine($"{User(uut.user_id)} is {uut.action}"); break;
                case UpdateChatUserTyping ucut: Console.WriteLine($"{Peer(ucut.from_id)} is {ucut.action} in {Chat(ucut.chat_id)}"); break;
                case UpdateChannelUserTyping ucut2: Console.WriteLine($"{Peer(ucut2.from_id)} is {ucut2.action} in {Chat(ucut2.channel_id)}"); break;
                case UpdateChatParticipants { participants: ChatParticipants cp }: Console.WriteLine($"{cp.participants.Length} participants in {Chat(cp.chat_id)}"); break;
                case UpdateUserStatus uus: Console.WriteLine($"{User(uus.user_id)} сейчас {uus.status.GetType().Name[10..]}"); break;
                case UpdateUserName uun: Console.WriteLine($"{User(uun.user_id)} поменял имя профиля: {uun.first_name} {uun.last_name}"); break;
                case UpdateUser uu: Console.WriteLine($"{User(uu.user_id)} has changed infos/photo"); break;
                default: Console.WriteLine(update.GetType().Name); break; // there are much more update types than the above example cases
            }
    }

    // показать сообщение в консоли
    Task DisplayMessage(MessageBase messageBase, bool edit = false)
    {
        if (edit) Console.Write("(Edit): ");
        switch (messageBase)
        {
            case Message m: Console.WriteLine($"{Peer(m.from_id) ?? m.post_author} in {Peer(m.peer_id)}> {m.message}"); break;
            case MessageService ms: Console.WriteLine($"{Peer(ms.from_id)} in {Peer(ms.peer_id)} [{ms.action.GetType().Name[13..]}]"); break;
        }
        return Task.CompletedTask;
    }


    string User(long id) => Users.TryGetValue(id, out var user) ? user.ToString() : $"Пользователь {id}";
    string Chat(long id) => Chats.TryGetValue(id, out var chat) ? chat.ToString() : $"Чат {id}";
    string Peer(Peer peer) => peer is null ? null : peer is PeerUser user ? User(user.user_id)
       : peer is PeerChat or PeerChannel ? Chat(peer.ID) : $"Peer {peer.ID}";

}
catch (Exception e) {Console.WriteLine(e.Message) ; loger.AddNewErrorRecord(e.Message); }
