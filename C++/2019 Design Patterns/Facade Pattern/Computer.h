//
// Created by gregathee on 12/22/2020.
//

#include "PCU.h"
#include "CPU.h"
#include "RAM.h"
#include "DiskDrive.h"
#include "Display.h"

#ifndef FACADE_PATTERN_COMPUTER_H
#define FACADE_PATTERN_COMPUTER_H

#endif //FACADE_PATTERN_COMPUTER_H

class Computer
{
private:
    PCU pcu;
    CPU cpu;
    RAM ram;
    DiskDrive diskDrive;
    Display display;
public:
    void PowerOn();
    void PowerOff();
    void StartProgram();
    void StopProgram();
};

