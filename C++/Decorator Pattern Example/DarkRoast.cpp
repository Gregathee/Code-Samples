#include "DarkRoast.h"
#include<iostream>

using std::cout;
using std::endl;

float DarkRoast::price = 2.25;

DarkRoast::DarkRoast () { description = "Dark Roast"; }

float DarkRoast::cost ( float addedCost ) { return price + addedCost; }