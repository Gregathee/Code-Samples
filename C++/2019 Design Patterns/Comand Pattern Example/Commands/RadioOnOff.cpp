#include "RadioOnOff.h"

void RadioOnOff::Execute ()
{
	isOn = !isOn;
	if ( isOn ) { std::cout << "Radio is on." << std::endl; }
	else { std::cout << "Radio is off." << std::endl; }
}

void RadioOnOff::Undo ()
{
	Execute ();
}

std::string RadioOnOff::Name ()
{
	return "Radio Switch";
}
