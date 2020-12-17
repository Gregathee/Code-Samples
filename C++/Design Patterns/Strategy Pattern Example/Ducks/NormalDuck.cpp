#include "NormalDuck.h"
#include<iostream>
#include"Squeak.h"
#include"Quack.h"
#include"Swear.h"
#include"CanFly.h"
#include"CannotFly.h"
#include"RocketPower.h"

using namespace std;

NormalDuck::NormalDuck ()
{
	quackingBehavior = new Quack ();
	flyingBehavior = new CanFly ();
}

void NormalDuck::callQuackBehavior () const
{
	quackingBehavior->quack ();
}

void NormalDuck::callFlyBehavior () const
{
	flyingBehavior->fly ();
}

void NormalDuck::display () const
{
	cout << "Im a Normal Duck\n";
}
