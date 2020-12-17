#pragma once
#include "Dough.h"
#include "Sauce.h"
#include "Topping.h"
#include "Cheese.h"
#include <vector> 
#include <iostream>
#include <string>
#include <ostream>

class Pizza
{
protected:
	Dough* PizzaDough;
	Sauce* PizzaSauce;
	std::vector<Topping*> toppings;
public:
	friend std::ostream& operator<< ( std::ostream& os, const  Pizza& pizza );
	std::string ToString () const;
	~Pizza ();
};