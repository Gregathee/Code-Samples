#include<iostream>
#include "Mocha.h"

using std::cout;
using std::endl;

float Mocha::price = 0.25f;

float Mocha::cost ( float addedCost ) { return beveragePtr->cost ( price + addedCost ); }

Mocha::Mocha ( Beverage* beveragePtrIn ) 
{
	description = "Mocha";
	beveragePtr = beveragePtrIn;
	beveragePtrIn = nullptr;
	appendDescription ( beveragePtr );
}
