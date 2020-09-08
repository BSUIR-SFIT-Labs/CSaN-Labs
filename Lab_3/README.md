# Программа, имитирующая работу протокола Finger
### Задание
Написать программу, реализующую следующие функции клиента и сервера протокола [Finger](https://tools.ietf.org/html/rfc1288).
### Описание
 - `FingerLib` - библиотека ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которая содержит основные классы для корректной работы приложения.
 - `FingerServer` - приложение ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которое реализует функции сервера [Finger](https://tools.ietf.org/html/rfc1288).
 - `FingerClient` - приложение ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которое реализует функции клиента [Finger](https://tools.ietf.org/html/rfc1288).
### Важные моменты
Программа реализована с использованием:
 - [Sockets](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=netcore-3.1)
 - [Asynchronous Client Socket](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example)
 - [Asynchronous Server Socket](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example)
