#include "ClassicChicagoStylePizza.h"
#include"ThickDough.h"
#include"MarinaraSauce.h"
#include"Cheese.h"
#include"Pepperoni.h"

ClassicChicagoStylePizza::ClassicChicagoStylePizza ()
{
	PizzaDough = new ThickDough ();
	PizzaSauce = new MarinaraSauce ();
	toppings.push_back ( new Cheese () );
	toppings.push_back ( new Pepperoni () );
}
