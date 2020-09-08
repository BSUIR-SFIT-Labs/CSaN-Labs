# Программа, позволяющая копировать бинарные файлы с одного компьютера на другой
### Задание
Написать программу, которая позволяет копировать бинарные файлы с одного компьютера на другой (**использовать протокол** [TCP](https://en.wikipedia.org/wiki/Transmission_Control_Protocol)).
### Описание
 - `FileSenderLib` - библиотека ([.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)), которая содержит основные классы для корректной работы приложения.
 - `Sender` - приложение ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которое позволяет отправлять файлы по сети.
 - `Listener` - приложение ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которое позволяет принимать файлы, отправленные приложением `Sender`.
### Важные моменты
Программа реализована с использованием:
 - [Sockets](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=netcore-3.1)
 - [Asynchronous Client Socket](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example)
 - [Asynchronous Server Socket](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example)
