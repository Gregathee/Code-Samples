#pragma once
#include"CondimentDecorator.h"

class Whip: public CondimentDecorator
{
public:
	static float price;
	Whip ( Beverage* beveragePtrIn );
	float cost ( float addedCost );
};
