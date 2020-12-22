#include "BlindsOpenClose.h"

void BlindsOpenClose::Execute ()
{
	isOn = !isOn;
	if ( isOn ) { std::cout << "Blinds are open." << std::endl; }
	else { std::cout << "Blinds are closed." << std::endl; }
}

void BlindsOpenClose::Undo ()
{
	Execute ();
}

std::string BlindsOpenClose::Name ()
{
	return "Blinds Switch";
}
