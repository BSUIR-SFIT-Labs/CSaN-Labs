# Программа, реализующая функции HTTP-клиента v1.0
### Задание
Написать программу, реализующую функции [HTTP-клиента версии 1.0](https://tools.ietf.org/html/rfc1945). В обязательном порядке должны поддерживаться следующие виды запросов: [GET](https://tools.ietf.org/html/rfc1945#section-8.1), [POST](https://tools.ietf.org/html/rfc1945#section-8.3), [HEAD](https://tools.ietf.org/html/rfc1945#section-8.2), а так же наиболее распространенные коды ответов. Отображение полученных данных в форматированном виде не обязательно (можно в виде plain text).В окне клиента должно быть расположено поле типа *memo* в котором отображается весь протокол общения HTTP-клиента с HTTP-сервером. Тестирование и подача HTTP-клиента производится при помощи запроса к реальному web-серверу, расположенному в Internet или установленному в локальной сети, или при помощи запрома к web-серверу, написанному в предыдущем задании.
### Описание
 - `HttpLib` - библиотека ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которая содержит основные классы для корректной работы приложения.
 - `HttpClientApp` - приложение ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)), которое реализует функции [HTTP-клиента](https://tools.ietf.org/html/rfc1945).
### Важные моменты
Программа реализована с использованием:
 - [TCP client](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netcore-3.1)
