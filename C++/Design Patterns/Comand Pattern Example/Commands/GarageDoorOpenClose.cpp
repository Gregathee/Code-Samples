#include "GarageDoorOpenClose.h"

void GarageDoorOpenClose::Execute ()
{
	isOn = !isOn;
	if ( isOn ) { std::cout << "Garage door is open." << std::endl; }
	else { std::cout << "Garage door is closed." << std::endl; }
}

void GarageDoorOpenClose::Undo ()
{
	Execute ();
}

std::string GarageDoorOpenClose::Name ()
{
	return "Garage Door Switch";
}
