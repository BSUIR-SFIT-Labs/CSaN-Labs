#pragma comment(lib, "Netapi32.lib")
#pragma comment(lib, "mpr.lib")

#include <Windows.h>
#include <winnetwk.h>
#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <string>

using namespace std;

void PrintAllActiveMacAddresses(void);
BOOL WINAPI EnumerateFunc(LPNETRESOURCE lpnr);

int main()
{
	// Output active MAC addresses.
	cout << "Mac addresses:" << endl;
	PrintAllActiveMacAddresses();
	cout << "----------------------------------------" << endl;

	// The output of all working groups on the network and their resources.
	cout << "Working Groups:" << endl;

	LPNETRESOURCE lpnr = NULL;

	if (EnumerateFunc(lpnr) == FALSE)
	{
		cout << "Call to EnumerateFunc failed" << endl;
	}
	cout << "----------------------------------------" << endl << endl;

	system("pause");
	return 0;
}


bool ResetAdapter(int adapterNumber) 
{
	NCB ncb;
	memset(&ncb, 0, sizeof(ncb));
	ncb.ncb_command = NCBRESET;
	ncb.ncb_lana_num = adapterNumber;

	if (Netbios(&ncb) == NRC_GOODRET)
	{
		return true;
	}

	return false;
}

BOOL GetAdapterMacAddress(int adapterNumber, string &macAddress) 
{
	if (ResetAdapter(adapterNumber))
	{
		// Prepare to get the adapter status block .
		NCB ncb;
		memset(&ncb, 0, sizeof(ncb));
		ncb.ncb_command = NCBASTAT;
		ncb.ncb_lana_num = adapterNumber;
		strcpy_s((char*)ncb.ncb_callname, sizeof(ncb.ncb_callname), "*");

		ADAPTER_STATUS adapterStatus;

		memset(&adapterStatus, 0, sizeof(adapterStatus));
		ncb.ncb_buffer = (unsigned char*)&adapterStatus;
		ncb.ncb_length = sizeof(adapterStatus);

		// Get the adapter's info and, if this works, return it in standard,
		// colon-delimited form.
		if (Netbios(&ncb) == 0)
		{
			char bufferForMacAddress[18];
			sprintf_s(bufferForMacAddress, sizeof(bufferForMacAddress), "%02X:%02X:%02X:%02X:%02X:%02X",
				int(adapterStatus.adapter_address[0]),
				int(adapterStatus.adapter_address[1]),
				int(adapterStatus.adapter_address[2]),
				int(adapterStatus.adapter_address[3]),
				int(adapterStatus.adapter_address[4]),
				int(adapterStatus.adapter_address[5]));
			macAddress = bufferForMacAddress;

			return TRUE;
		}

		return FALSE;
	}

	return FALSE;
}

LANA_ENUM GetAdapterList(void) 
{
	LANA_ENUM adapterList;
	NCB ncb;

	memset(&ncb, 0, sizeof(NCB));
	ncb.ncb_command = NCBENUM;
	ncb.ncb_buffer = (unsigned char*)&adapterList;
	ncb.ncb_length = sizeof(adapterList);

	Netbios(&ncb);

	return adapterList;
}

void PrintAllActiveMacAddresses(void)
{
	const LANA_ENUM adapterList = GetAdapterList();

	// Get all of the local ethernet addresses.
	string macAddress;

	for (int i = 0; i < adapterList.length; i++)
	{
		if (GetAdapterMacAddress(adapterList.lana[i], macAddress))
		{
			cout << "Adapter " << int(adapterList.lana[i]) <<
				"'s MAC is " << macAddress << endl;
		}
		else
		{
			cout << "Failed to get MAC address! Do you have the NetBIOS protocol installed?"
				<< endl;
			break;
		}
	}
}

void DisplayStruct(int i, LPNETRESOURCE lpnrLocal)
{
	printf("NETRESOURCE[%d] Scope: ", i);
	switch (lpnrLocal->dwScope) 
	{
		case (RESOURCE_CONNECTED):
			printf("connected\n");
			break;
		case (RESOURCE_GLOBALNET):
			printf("all resources\n");
			break;
		case (RESOURCE_REMEMBERED):
			printf("remembered\n");
			break;
		default:
			printf("unknown scope %d\n", lpnrLocal->dwScope);
			break;
	}

	printf("NETRESOURCE[%d] Type: ", i);
	switch (lpnrLocal->dwType) 
	{
		case (RESOURCETYPE_ANY):
			printf("any\n");
			break;
		case (RESOURCETYPE_DISK):
			printf("disk\n");
			break;
		case (RESOURCETYPE_PRINT):
			printf("print\n");
			break;
		default:
			printf("unknown type %d\n", lpnrLocal->dwType);
			break;
	}

	printf("NETRESOURCE[%d] DisplayType: ", i);
	switch (lpnrLocal->dwDisplayType) 
	{
		case (RESOURCEDISPLAYTYPE_GENERIC):
			printf("generic\n");
			break;
		case (RESOURCEDISPLAYTYPE_DOMAIN):
			printf("domain\n");
			break;
		case (RESOURCEDISPLAYTYPE_SERVER):
			printf("server\n");
			break;
		case (RESOURCEDISPLAYTYPE_SHARE):
			printf("share\n");
			break;
		case (RESOURCEDISPLAYTYPE_FILE):
			printf("file\n");
			break;
		case (RESOURCEDISPLAYTYPE_GROUP):
			printf("group\n");
			break;
		case (RESOURCEDISPLAYTYPE_NETWORK):
			printf("network\n");
			break;
		default:
			printf("unknown display type %d\n", lpnrLocal->dwDisplayType);
			break;
	}

	printf("NETRESOURCE[%d] Usage: 0x%x = ", i, lpnrLocal->dwUsage);
	if (lpnrLocal->dwUsage & RESOURCEUSAGE_CONNECTABLE)
	{
		printf("connectable ");
	}
	if (lpnrLocal->dwUsage & RESOURCEUSAGE_CONTAINER) 
	{
		printf("container ");
	}
	printf("\n");

	printf("NETRESOURCE[%d] Localname: %S\n", i, lpnrLocal->lpLocalName);
	printf("NETRESOURCE[%d] Remotename: %S\n", i, lpnrLocal->lpRemoteName);
	printf("NETRESOURCE[%d] Comment: %S\n", i, lpnrLocal->lpComment);
	printf("NETRESOURCE[%d] Provider: %S\n", i, lpnrLocal->lpProvider);
	printf("\n");
}

BOOL WINAPI EnumerateFunc(LPNETRESOURCE lpnr)
{
	DWORD dwResult, dwResultEnum;
	HANDLE hEnum;
	DWORD cbBuffer = 16384;  
	DWORD cEntries = -1;       
	LPNETRESOURCE lpnrLocal;    
	DWORD i;

	// Call the WNetOpenEnum function to begin the enumeration.
	dwResult = WNetOpenEnum(RESOURCE_GLOBALNET, // all network resources
		RESOURCETYPE_ANY,                       // all resources
		0,                                      // enumerate all resources
		lpnr,                                   // NULL first time the function is called
		&hEnum);                                // handle to the resource

	if (dwResult != NO_ERROR) 
	{
		printf("WnetOpenEnum failed with error %d\n", dwResult);
		return FALSE;
	}
	
	// Call the GlobalAlloc function to allocate resources.
	lpnrLocal = (LPNETRESOURCE)GlobalAlloc(GPTR, cbBuffer);
	if (lpnrLocal == NULL) 
	{
		printf("WnetOpenEnum failed with error %d\n", dwResult);

		return FALSE;
	}

	do 
	{
		// Initialize the buffer.
		ZeroMemory(lpnrLocal, cbBuffer);
		
		// Call the WNetEnumResource function to continue
		// the enumeration.
		dwResultEnum = WNetEnumResource(hEnum,  // resource handle
			&cEntries,                          // defined locally as -1
			lpnrLocal,                          // LPNETRESOURCE
			&cbBuffer);                         // buffer size

        // If the call succeeds, loop through the structures.
		if (dwResultEnum == NO_ERROR) 
		{
			for (i = 0; i < cEntries; i++) 
			{
				// Call an application-defined function to
				// display the contents of the NETRESOURCE structures.
				DisplayStruct(i, &lpnrLocal[i]);

				// If the NETRESOURCE structure represents a container resource, 
				// call the EnumerateFunc function recursively.

				if (RESOURCEUSAGE_CONTAINER == (lpnrLocal[i].dwUsage
					& RESOURCEUSAGE_CONTAINER))
				{
					if (!EnumerateFunc(&lpnrLocal[i]))
					{
						printf("EnumerateFunc returned FALSE\n");
					}
				}
			}
		}

		// Process errors.
		else if (dwResultEnum != ERROR_NO_MORE_ITEMS) 
		{
			printf("WNetEnumResource failed with error %d\n", dwResultEnum);
			break;
		}
	} while (dwResultEnum != ERROR_NO_MORE_ITEMS);
	
	// Call the GlobalFree function to free the memory.
	GlobalFree((HGLOBAL)lpnrLocal);
	
	// Call WNetCloseEnum to end the enumeration.
	dwResult = WNetCloseEnum(hEnum);

	if (dwResult != NO_ERROR)
	{
		// Process errors.
		printf("WNetCloseEnum failed with error %d\n", dwResult);
		
		return FALSE;
	}

	return TRUE;
}
