#include "GooseToDuck.h"

void GooseToDuck::callQuackBehavior () const
{
	if ( quackingBehavior ) { Duck::quackingBehavior->quack (); }
	else { goose->honk (); }
}

void GooseToDuck::callFlyBehavior () const
{
	if ( flyingBehavior ) { Duck::flyingBehavior->fly (); }
	else { goose->fly (); }
}

void GooseToDuck::display () const { goose->display (); }

void GooseToDuck::swim () const { goose->swim (); }

GooseToDuck::GooseToDuck ( Goose* goose ) { this->goose = goose; }
