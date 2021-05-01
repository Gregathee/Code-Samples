#include "PizzaPlace.h"

void PizzaPlace::OrderPizza ( std::string pizza )
{
	Pizza* orderedPizza ( &CreatePizza ( pizza ) );
	std::cout << "Cooking " << *orderedPizza << std::endl;
}
