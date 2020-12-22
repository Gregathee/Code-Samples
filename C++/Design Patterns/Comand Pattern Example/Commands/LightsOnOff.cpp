#include "LightsOnOff.h"

void LightsOnOff::Execute ()
{
	isOn = !isOn;
	if ( isOn ) { std::cout << "Lights is on." << std::endl; }
	else { std::cout << "Lights is off." << std::endl; }
}

void LightsOnOff::Undo ()
{
	Execute ();
}

std::string LightsOnOff::Name ()
{
	return "Light Switch";
}
