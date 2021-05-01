#pragma once
#include "Pizza.h"

class PizzaPlace
{
private:
	virtual Pizza& CreatePizza ( const std::string& pizza ) const = 0;
public:
	void OrderPizza ( std::string pizza );
};