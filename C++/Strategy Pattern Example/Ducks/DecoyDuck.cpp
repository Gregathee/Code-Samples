#include "DecoyDuck.h"
#include<iostream>
#include"Squeak.h"
#include"Quack.h"
#include"Swear.h"
#include"CanFly.h"
#include"CannotFly.h"
#include"RocketPower.h"


using namespace std;

DecoyDuck::DecoyDuck ()
{
	quackingBehavior = new Quack ();
	flyingBehavior = new CannotFly ();
}

void DecoyDuck::callQuackBehavior () const
{
	quackingBehavior->quack ();
}

void DecoyDuck::callFlyBehavior () const
{
	flyingBehavior->fly ();
}

void DecoyDuck::display () const
{
	cout << "I'm a decoy Duck.\n";
}
