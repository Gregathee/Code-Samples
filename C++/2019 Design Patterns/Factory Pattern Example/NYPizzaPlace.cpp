#include <iostream>
#include "NYPizzaPlace.h"
#include"CheeseNYStylePizza.h"
#include"ClassicNYStylePizza.h"

using std::cout;

Pizza& NYPizzaPlace::CreatePizza ( const std::string& pizza ) const
{
	Pizza* pizzaPtr = nullptr;
	if ( pizza == "Cheese" ) { pizzaPtr = new CheesseNYStylePizza (); }
	else if ( pizza == "Classic" ) { pizzaPtr = new ClassicNYStylePizza (); }
	return *pizzaPtr;
}
