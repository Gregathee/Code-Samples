#pragma once
#include"Duck.h"

class PottyMouthDuck : public Duck
{
public:
	PottyMouthDuck ();
	virtual void callQuackBehavior () const override;
	virtual void callFlyBehavior () const override;
	virtual void display () const override;
};