## Program for displaying active MAC-addresses and work groups with resources

##### Detailed description:
This program displays the active MAC-address(es) of the computer and displays all work groups, computers on the network and their resources (folders open for public access, printers).

##### Important points:
Getting MAC-addresses is implemented using ***`netbios`*** functionality.</br>
Obtaining work groups, computers on the network and their resources is implemented using ***`WNet`*** functions.

##### Documentation:
`Netbios documentation:`[ https://docs.microsoft.com/en-us/windows/win32/api/_netbios/][1] </br>
`WNet documentation:`[https://docs.microsoft.com/en-us/windows/win32/wnet/enumerating-network-resources][2]

[1]: https://docs.microsoft.com/en-us/windows/win32/api/_netbios/
[2]: https://docs.microsoft.com/en-us/windows/win32/wnet/enumerating-network-resources