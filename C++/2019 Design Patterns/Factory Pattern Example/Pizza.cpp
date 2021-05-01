#include "Pizza.h"

std::string Pizza::ToString () const
{
    std::string toppingString;
    for ( Topping* topping : toppings )  toppingString += topping->name + " ";
     return PizzaDough->name + " crust with " + PizzaSauce->name + " " + toppingString;
}

Pizza::~Pizza ()
{
    delete PizzaDough;
    delete PizzaSauce;
    for ( Topping* topping : toppings ) { delete topping; }
    std::cout << "Pizza Destroyed" << std::endl;
}

std::ostream& operator<<( std::ostream& os, const Pizza& pizza )
{
    os << pizza.ToString ();
    return os;
}
