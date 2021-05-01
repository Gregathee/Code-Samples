#include "Decaf.h"
#include<iostream>

using std::cout;
using std::endl;

float Decaf::price = 2.30;

Decaf::Decaf () { description = "Decaf"; }

float Decaf::cost ( float addedCost ) { return price + addedCost; }
