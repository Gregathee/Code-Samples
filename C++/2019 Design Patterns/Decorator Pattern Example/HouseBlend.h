#pragma once
#include "Beverage.h"

class HouseBlend: public Beverage
{
public:
	static float price;
	HouseBlend ();
	float cost ( float addedCost );
};