#include "CheeseNYStylePizza.h"
#include"AlfradoSauce.h"
#include"ThinDough.h"
#include"Cheese.h"

CheesseNYStylePizza::CheesseNYStylePizza ()
{
	PizzaDough = new ThinDough ();
	PizzaSauce = new AlfradoSauce ();
	toppings.push_back ( new Cheese () );
}
