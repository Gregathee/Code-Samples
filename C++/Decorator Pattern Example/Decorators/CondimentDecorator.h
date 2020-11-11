#pragma once
#include"Beverage.h"

class CondimentDecorator: public Beverage
{
protected:
	Beverage* beveragePtr;
public:
	virtual float cost ( float addedCost )=0;
};
