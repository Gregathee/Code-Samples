#include<iostream>
#include "Goose.h"

using std::cout;
using std::endl;

void Goose::honk () const { std::cout << "Honk Honk." << endl; }

void Goose::fly () const { cout << "Fly." << endl; }

void Goose::display () const { cout << "I'm a goose." << endl; }

void Goose::swim () const { cout << "Swim." << endl; }
