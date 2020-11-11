#include "HouseBlend.h"
#include<iostream>

using std::cout;
using std::endl;

float HouseBlend::price = 2.25;

HouseBlend::HouseBlend () { description = "House Blend"; }

float HouseBlend::cost ( float addedCost ) { return price + addedCost; }
