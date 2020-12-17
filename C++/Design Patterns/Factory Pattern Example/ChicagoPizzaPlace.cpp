#include"ChicagoPizzaPlace.h"
#include"CheeseChicagoStylePizza.h"
#include"ClassicChicagoStylePizza.h"

Pizza& ChicagoPizzaPlace::CreatePizza ( const std::string& pizza ) const
{
	Pizza* pizzaPtr = nullptr;
	if ( pizza == "Cheese" ) { pizzaPtr = new CheeseChicagoStylePizza (); }
	else if ( pizza == "Classic" ) { pizzaPtr = new ClassicChicagoStylePizza (); }
	return *pizzaPtr;
}
