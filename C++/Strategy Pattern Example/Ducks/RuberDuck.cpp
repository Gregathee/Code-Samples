#include "RuberDuck.h"
#include"Quack.h"
#include<iostream>
#include"Squeak.h"
#include"Quack.h"
#include"Swear.h"
#include"CanFly.h"
#include"CannotFly.h"
#include"RocketPower.h"

using namespace std;

RuberDuck::RuberDuck ()
{
	quackingBehavior = new Squeak ();
	flyingBehavior = new CannotFly ();
}

void RuberDuck::callQuackBehavior () const
{
	quackingBehavior->quack ();
}

void RuberDuck::display () const
{
	flyingBehavior->fly ();
}

void RuberDuck::callFlyBehavior () const
{
	cout << "I'm a Rubber duck.\n";
}
