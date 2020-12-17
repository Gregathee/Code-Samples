#pragma once
#include "Beverage.h"

class DarkRoast: public Beverage
{
public:
	static float price;
	DarkRoast ();
	float cost ( float addedCost );
};
