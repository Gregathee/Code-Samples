#include<iostream>
#include "Milk.h"

using std::cout;
using std::endl;

float Milk::price = 0.30;

float Milk::cost ( float addedCost ) { return beveragePtr->cost ( price + addedCost ); }

Milk::Milk ( Beverage* beveragePtrIn )
{
	description = "Milk";
	beveragePtr = beveragePtrIn;
	beveragePtrIn = nullptr;
	appendDescription ( beveragePtr );
}