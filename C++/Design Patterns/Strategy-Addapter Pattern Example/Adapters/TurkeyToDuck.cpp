#include "TurkeyToDuck.h"
#include<iostream>

void TurkeyToDuck::callQuackBehavior () const
{
	if ( quackingBehavior ) { Duck::quackingBehavior->quack (); }
	else { turkey->gobble (); }
}

void TurkeyToDuck::callFlyBehavior () const
{
	if ( flyingBehavior ) { Duck::flyingBehavior->fly (); }
	else { turkey->fly (); }
}

void TurkeyToDuck::display () const { turkey->display (); }

void TurkeyToDuck::swim () const { std::cout << "I can't swim" << std::endl; }

TurkeyToDuck::TurkeyToDuck ( Turkey* turkey ) { this->turkey = turkey; }
