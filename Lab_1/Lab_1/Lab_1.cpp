#pragma comment(lib, "Netapi32.lib")

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <Windows.h>

using namespace std;

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

bool GetAdapterMacAddress(int adapterNumber, string &macAddress) 
{
	if (ResetAdapter(adapterNumber))
	{
		// Prepare to get the adapter status block 
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

			return true;
		}

		return false;
	}

	return false;
}

LANA_ENUM GetAdapterList() 
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

void PrintAllMacAddresses()
{
	const LANA_ENUM adapterList = GetAdapterList();

	// Get all of the local ethernet addresses
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
			cerr << "Failed to get MAC address! Do you" << endl;
			cerr << "have the NetBIOS protocol installed?" << endl;
			break;
		}
	}
}

int main()
{
	PrintAllMacAddresses();

	system("pause");
	return 0;
}
