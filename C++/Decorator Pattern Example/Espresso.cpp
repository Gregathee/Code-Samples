#include "Espresso.h"
#include<iostream>

using std::cout;
using std::endl;

float Espresso::price = 0.75;

Espresso::Espresso () { description = "Espresso"; }

float Espresso::cost ( float addedCost ) { return price + addedCost; }
