#include "TvOnOff.h"

void TvOnOff::Execute ()
{
	isOn = !isOn;
	if ( isOn ) { std::cout << "TV is on." << std::endl; }
	else { std::cout << "TV is off." << std::endl; }
}

void TvOnOff::Undo ()
{
	Execute ();
}

std::string TvOnOff::Name ()
{
	return "TV Switch";
}
