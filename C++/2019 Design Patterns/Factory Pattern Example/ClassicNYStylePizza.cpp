#include "ClassicNYStylePizza.h"
#include"AlfradoSauce.h"
#include"ThinDough.h"
#include"Sausage.h"

ClassicNYStylePizza::ClassicNYStylePizza ()
{
	PizzaDough = new ThinDough ();
	PizzaSauce = new AlfradoSauce ();
	toppings.push_back ( new Cheese () );
	toppings.push_back ( new Sausage () );
}
