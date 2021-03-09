//
// Created by gregathee on 12/22/2020.
//

#include <iostream>
#include "Headers/Computer.h"

void Computer::PowerOn()
{
    pcu.DrawPower();
    std::cout << std::endl << "[Computer] Starting OS" << std::endl << std::endl;
    StartProgram();
}

void Computer::StartProgram()
{
    diskDrive.ReadFromDisk();
    std::cout << std::endl;
    ram.StoreMemory();
    ram.RetrieveMemory();
    std::cout << std::endl;
    cpu.ProcessInfo();
    std::cout << std::endl;
    display.SendDisplayInfo();
    std::cout << std::endl;
    ram.StoreMemory();
}

void Computer::StopProgram()
{
    ram.StoreMemory();
    ram.RetrieveMemory();
    std::cout << std::endl;
    cpu.ProcessInfo();
    std::cout << std::endl;
    display.SendDisplayInfo();
    std::cout << std::endl;
    diskDrive.WriteToDisk();
}

void Computer::PowerOff()
{
    std::cout << "[Computer] Stopping OS" << std::endl << std::endl;
    StopProgram();
    std::cout << std:: endl;
    pcu.CutPower();
}

