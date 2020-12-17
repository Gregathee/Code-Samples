#pragma once
#include "PizzaPlace.h"

class NYPizzaPlace: public PizzaPlace
{
public:
	virtual Pizza& CreatePizza (const std::string& pizza) const override;
};