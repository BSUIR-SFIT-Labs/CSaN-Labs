# Программа, отображающая MAC-адреса и рабочии группы с их ресурсами

### Задание
Написать программу, реализующую следующие функции (**использовать только функции** [WinApi](https://docs.microsoft.com/en-us/windows/win32/apiindex/windows-api-list)):
1. Отображение [MAC-адреса](https://en.wikipedia.org/wiki/MAC_address) компьютера (можно воспользоваться функцией [netbios](https://docs.microsoft.com/en-us/windows/win32/api/_netbios/)).
2. Отображение всех `рабочих групп`, `компьютеров в сети` и их `ресурсов` (папок, открытых для общего доступа, принтеров). Воспользоваться функциями [WNet](https://docs.microsoft.com/en-us/windows/win32/wnet/wnet-functions).

### Важные моменты
 - Получение [MAC-адресов](https://en.wikipedia.org/wiki/MAC_address) реализовано с помощью функций [netbios](https://docs.microsoft.com/en-us/windows/win32/api/_netbios/).
 - Получение `рабочих групп`, `компьютеров в сети` и их `ресурсов` реализовано с помощью функций [WNet](https://docs.microsoft.com/en-us/windows/win32/wnet/wnet-functions).
