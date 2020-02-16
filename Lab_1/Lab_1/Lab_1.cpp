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
	cout << "NETRESOURCE[" << i << "] Scope: ";
	switch (lpnrLocal->dwScope) {
	case (RESOURCE_CONNECTED):
		cout << "connected" << endl;
		break;
	case (RESOURCE_GLOBALNET):
		cout << "all resources" << endl;
		break;
	case (RESOURCE_REMEMBERED):
		cout << "remembered" << endl;
		break;
	default:
		cout << "unknown scope " << lpnrLocal->dwScope << endl;
		break;
	}

	cout << "NETRESOURCE[" << i << "] Type: ";
	switch (lpnrLocal->dwType) {
	case (RESOURCETYPE_ANY):
		cout << "any" << endl;
		break;
	case (RESOURCETYPE_DISK):
		cout << "disk" << endl;
		break;
	case (RESOURCETYPE_PRINT):
		cout << "print" << endl;
		break;
	default:
		cout << "unknown type " << lpnrLocal->dwType << endl;
		break;
	}

	cout << "NETRESOURCE[" << i << "] DisplayType: ";
	switch (lpnrLocal->dwDisplayType) {
	case (RESOURCEDISPLAYTYPE_GENERIC):
		cout << "generic\n";
		break;
	case (RESOURCEDISPLAYTYPE_DOMAIN):
		cout << "domain" << endl;
		break;
	case (RESOURCEDISPLAYTYPE_SERVER):
		cout << "server" << endl;
		break;
	case (RESOURCEDISPLAYTYPE_SHARE):
		cout << "share" << endl;
		break;
	case (RESOURCEDISPLAYTYPE_FILE):
		cout << "file" << endl;
		break;
	case (RESOURCEDISPLAYTYPE_GROUP):
		cout << "group" << endl;
		break;
	case (RESOURCEDISPLAYTYPE_NETWORK):
		cout << "network" << endl;
		break;
	default:
		cout << "unknown display type " << lpnrLocal->dwDisplayType << endl;
		break;
	}

	cout << "NETRESOURCE[" << i << "] Usage: " << lpnrLocal->dwUsage << " = ";
	if (lpnrLocal->dwUsage & RESOURCEUSAGE_CONNECTABLE)
		cout << "connectable";
	if (lpnrLocal->dwUsage & RESOURCEUSAGE_CONTAINER)
		cout << "container";
	cout << endl;

	cout << "NETRESOURCE[" << i << "] Localname: " << lpnrLocal->lpLocalName << endl;
	cout << "NETRESOURCE[" << i << "] Remotename: " << lpnrLocal->lpRemoteName << endl;
	cout << "NETRESOURCE[" << i << "] Comment: " << lpnrLocal->lpComment << endl;
	cout << "NETRESOURCE[" << i << "] Provider: " << lpnrLocal->lpProvider << endl;
	cout << endl;
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
		cout << "WnetOpenEnum failed with error " << dwResult << endl;
		return FALSE;
	}

	// Call the GlobalAlloc function to allocate resources.
	lpnrLocal = (LPNETRESOURCE)GlobalAlloc(GPTR, cbBuffer);
	if (lpnrLocal == NULL) 
	{
		cout << "WnetOpenEnum failed with error " << dwResult << endl;
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
					if (!EnumerateFunc(&lpnrLocal[i]))
						cout << "EnumerateFunc returned FALSE" << endl << endl;
			}
		}

		// Process errors.
		else if (dwResultEnum != ERROR_NO_MORE_ITEMS) 
		{
			cout << "WNetEnumResource failed with error " << dwResultEnum << endl;
			break;
		}
	} while (dwResultEnum != ERROR_NO_MORE_ITEMS);

	// Call the GlobalFree function to free the memory.
	GlobalFree((HGLOBAL)lpnrLocal);

	// Call WNetCloseEnum to end the enumeration.
	dwResult = WNetCloseEnum(hEnum);

	if (dwResult != NO_ERROR) {
	
		// Process errors.
		cout << "WNetCloseEnum failed with error " << dwResult << endl;

		return FALSE;
	}

	return TRUE;
}
