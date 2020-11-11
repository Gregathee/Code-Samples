#include<iostream>
#include<string>
#include"NormalDuck.h"
#include"PottyMouthDuck.h"
#include"RuberDuck.h"
#include"DecoyDuck.h"
#include"Squeak.h"
#include"Quack.h"
#include"Swear.h"
#include"CanFly.h"
#include"CannotFly.h"
#include"RocketPower.h"

using std::cout;
using std::endl;

void callDuckMethods (Duck* duck);

int main ()
{
	FlyingBehavior* flyBehavior = new CannotFly ();
	QuackingBehavior* quackBehavior = new Squeak ();

	Duck* duck1 = new NormalDuck();
	Duck* duck2 = new DecoyDuck ();
	Duck* duck3 = new RuberDuck ();
	Duck* duck4 = new PottyMouthDuck ();
	cout << "Normal duck:\n\n";
	callDuckMethods ( duck1 );

	cout << "\nNormal duck will now squeek and won't be able to fly.\n\n";

	duck1->setFlyBehavior ( flyBehavior );
	duck1->setQuackBehavior ( quackBehavior );
	callDuckMethods ( duck1 );

	cout << endl;

	cout << "Decoy duck:\n\n";
	callDuckMethods ( duck2 );

	cout << endl;

	
	cout << "Rubber duck:\n\n";
	callDuckMethods ( duck3 );

	cout << endl;

	
	cout << "Potty mouth duck:\n\n";
	callDuckMethods ( duck4 );

	cout << endl;

	delete duck1, duck2, duck3, duck4;

	system ( "pause" );
}

void callDuckMethods ( Duck * duck )
{
	duck->callFlyBehavior ();
	duck->callQuackBehavior ();
	duck->display ();
}
