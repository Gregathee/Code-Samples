#pragma once
#include "PizzaPlace.h"

class ChicagoPizzaPlace: public PizzaPlace
{
public:
	virtual Pizza& CreatePizza ( const std::string& pizza ) const override;
};