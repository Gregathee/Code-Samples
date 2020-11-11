#pragma once
#include"CondimentDecorator.h"

class Milk: public CondimentDecorator
{
public:
	static float price;
	Milk ( Beverage*  beveragePtrIn );
	virtual float cost ( float addedCost ) override;
};
