#include "PottyMouthDuck.h"
#include<iostream>
#include"Squeak.h"
#include"Quack.h"
#include"Swear.h"
#include"CanFly.h"
#include"CannotFly.h"
#include"RocketPower.h"

using namespace std;

PottyMouthDuck::PottyMouthDuck ()
{
	quackingBehavior = new Swear ();
	flyingBehavior = new RocketPower ();
}

void PottyMouthDuck::callQuackBehavior () const
{
	quackingBehavior->quack ();
}

void PottyMouthDuck::callFlyBehavior () const
{
	flyingBehavior->fly ();
}

void PottyMouthDuck::display () const
{
	cout << "I'm a potty mouth duck.\n";
}
