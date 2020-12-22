#include "Turkey.h"
#include<iostream>

using std::cout;
using std::endl;

void Turkey::gobble () const { cout << "Gobble Gobble." << endl; }

void Turkey::fly () const { cout << "Fly...but not very well..." << endl; }

void Turkey::display () const { cout << "I'm a turkey." << endl;}
