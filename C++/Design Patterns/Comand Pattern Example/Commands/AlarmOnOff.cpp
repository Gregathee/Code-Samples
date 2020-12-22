#include "AlarmOnOff.h"

void AlarmOnOff::Execute ()
{
	isOn = !isOn;
	if ( isOn ) { std::cout << "Alarm is on." << std::endl; }
	else { std::cout << "Alarm is off." << std::endl; }
}

void AlarmOnOff::Undo ()
{
	Execute ();
}

std::string AlarmOnOff::Name ()
{
	return "Alarm Switch";
}
