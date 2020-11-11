#include "Duck.h"
#include<iostream>
#include"Squeak.h"
#include"Quack.h"
#include"Swear.h"
#include"CanFly.h"
#include"CannotFly.h"
#include"RocketPower.h"

using namespace std;

void Duck::swim () const
{
	cout << "Swim\n";
}

void Duck::setQuackBehavior ( QuackingBehavior*& quackIn )
{
	quackingBehavior = quackIn;
}

void Duck::setFlyBehavior ( FlyingBehavior *& flyIn )
{
	flyingBehavior = flyIn;
}
