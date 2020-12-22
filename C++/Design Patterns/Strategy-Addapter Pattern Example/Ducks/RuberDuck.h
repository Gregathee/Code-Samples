#pragma once
#include"Duck.h"

class RuberDuck : public Duck
{
public:
	RuberDuck (); 
	virtual void callQuackBehavior () const override;
	virtual void callFlyBehavior () const override;
	virtual void display () const override;
};