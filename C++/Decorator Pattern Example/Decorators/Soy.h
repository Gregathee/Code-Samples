#pragma once
#include"CondimentDecorator.h"

class Soy: public CondimentDecorator
{
public:
	static float price;
	Soy ( Beverage* beveragePtr );
	float cost ( float addedCost );
};