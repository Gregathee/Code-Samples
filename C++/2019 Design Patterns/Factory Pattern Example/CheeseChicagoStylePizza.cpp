#include "CheeseChicagoStylePizza.h"
#include"ThickDough.h"
#include"MarinaraSauce.h"
#include"Cheese.h"

CheeseChicagoStylePizza::CheeseChicagoStylePizza ()
{
	PizzaDough = new ThickDough ();
	PizzaSauce = new MarinaraSauce ();
	toppings.push_back ( new Cheese () );
}
