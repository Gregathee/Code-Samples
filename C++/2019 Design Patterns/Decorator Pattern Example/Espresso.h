#pragma once
#include "Beverage.h"

class Espresso: public Beverage
{
private:
	std::string description = "Espresso";
	
public:
	static float price;
	Espresso ();
	float cost ( float addedCost );
};
