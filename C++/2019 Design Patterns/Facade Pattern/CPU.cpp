//
// Created by gregathee on 12/23/2020.
//

#include "Headers/CPU.h"
#include <iostream>

using std::cout;
using std::endl;

void CPU::SendToCache()
{
    cout << "[CPU] Writing to cache" << endl;
}

void CPU::RetreiveFromCache()
{
    cout << "[CPU] Reading from cache" << endl;
}

void CPU::ProcessJob()
{
    cout << "[CPU] Processing job" << endl;
}

void CPU::ProcessInfo()
{
    RetreiveFromCache();
    ProcessJob();
    SendToCache();
}

