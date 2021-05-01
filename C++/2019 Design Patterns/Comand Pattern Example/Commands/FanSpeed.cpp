#include "FanSpeed.h"

void FanSpeed::PrintSpeed ()
{
	switch ( speed )
	{
	case 0: std::cout << "Fan is off." << std::endl; break;
	case 1: std::cout << "Fan is on slow." << std::endl; break;
	case 2: std::cout << "Fan is on medium." << std::endl; break;
	case 3: std::cout << "Fan is on high." << std::endl; break;
	}
}

void FanSpeed::Execute ()
{
	speed++;
	if ( speed == 4 ) { speed = 0; }
	PrintSpeed ();
}

void FanSpeed::Undo ()
{
	speed--;
	if ( speed == -1 ) { speed = 3; }
	PrintSpeed ();
}

std::string FanSpeed::Name ()
{
	return "Fan Speed";
}
