#include<iostream>
#include "Soy.h"

using std::cout;
using std::endl;

float Soy::price = 0.40f;

float Soy::cost ( float addedCost ) { return beveragePtr->cost ( price + addedCost ); }

Soy::Soy ( Beverage* beveragePtrIn ) 
{	
	description = "Soy";
	beveragePtr = beveragePtrIn;
	beveragePtrIn = nullptr;
	appendDescription ( beveragePtr ); 
}
