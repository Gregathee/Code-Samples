#pragma once
#include "Beverage.h"

class Decaf: public Beverage
{
public:
	static float price;
	Decaf ();
	float cost ( float addedCost );
};
