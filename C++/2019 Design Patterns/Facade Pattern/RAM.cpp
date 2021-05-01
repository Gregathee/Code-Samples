//
// Created by gregathee on 12/23/2020.
//

#include <iostream>
#include "Headers/RAM.h"

using std::cout;
using std::endl;

void RAM::StoreMemory()
{
    AllocateMemory();
    WriteMemory();
}

void RAM::AllocateMemory()
{
    cout << "[RAM] Allocating Memory" << endl;
}

void RAM::WriteMemory()
{
    cout << "[RAM] Writing to RAM" << endl;
}

void RAM::SearchForData()
{
    cout << "[RAM] Searching for data" << endl;
}

void RAM::SendToCPU()
{
    cout << "[RAM] Sending to CPU" << endl;
}

void RAM::RetrieveMemory()
{
    SearchForData();
    SendToCPU();
}