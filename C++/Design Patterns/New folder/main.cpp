#include <iostream>
#include "Headers/Computer.h"

using std::cout;
using std::endl;

int main() {
    Computer computer = Computer();
    cout << "[User] Powering on computer" << endl << endl;
    computer.PowerOn();
    cout << endl;
    cout << "[User] Starting Program" << endl << endl;
    computer.StartProgram();
    cout << endl;
    cout << "[User] Stopping program" << endl << endl;
    computer.StopProgram();
    cout << endl;
    cout << "[User] Powering off computer" << endl << endl;
    computer.PowerOff();
    return 0;
}
