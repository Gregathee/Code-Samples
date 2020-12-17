#include<iostream>
#include "Whip.h"

using std::cout;
using std::endl;

float Whip::price = 0.25f;

float Whip::cost ( float addedCost ) { return beveragePtr->cost ( price + addedCost ); }

Whip::Whip ( Beverage*  beveragePtrIn ) 
{
	description = "Whip";
	beveragePtr = beveragePtrIn;
	beveragePtrIn = nullptr;
	appendDescription ( beveragePtr );
}
	


