#pragma once
#include"CondimentDecorator.h"

class Mocha: public CondimentDecorator
{
public:
	static float price;
	Mocha ( Beverage* beveragePtrIn );
	float cost ( float addedCost );
};