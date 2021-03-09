//
// Created by gregathee on 12/23/2020.
//

#include <iostream>
#include "Headers/DiskDrive.h"
using std::cout;
using std::endl;

void DiskDrive::ReadFromDisk()
{
    SearchForData();
    LoadData();
    SendToRam();
}

void DiskDrive::SendToRam()
{
    cout << "[Disk Drive] Sending to RAM" << endl;
}

void DiskDrive::LoadData()
{
    cout << "[Disk Drive] Loading Data" << endl;
}

void DiskDrive::SearchForData()
{
    cout << "[Disk Drive] Searching for data" << endl;
}

void DiskDrive::WriteToDisk()
{
    AllocateMemory();
    Write();
}

void DiskDrive::AllocateMemory()
{
    cout << "[Disk Drive] Allocating Memory" << endl;
}

void DiskDrive::Write()
{
    cout << "[Disk Drive] Writing to Disk" << endl;
}

