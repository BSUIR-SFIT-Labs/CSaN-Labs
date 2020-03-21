## Program for sending/receiving files over a network using TCP
#### Detailed description:
These programs allow to `send` or `receive` files over the network using the ***`TCP`*** protocol.

The `FileSenderLib` folder contains the main classes for the correct operation of the application.

In the `Sender` folder is an application that allows to send files over the network.

In the `Listener` folder is an application that allows to receive files over the network.

#### Important points:
The `FileSenderLib` class library is implemented on ***`.NET Standard 2.0`***.

`Sender` and `Listener` are implemented on ***`.NET Core 3.1`***.

Sending and receiving files is implemented using ***`sockets`***.

#### Documentation:
`Sockets`: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/sockets

`Asynchronous Client Socket`: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example

`Asynchronous Server Socket`: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
