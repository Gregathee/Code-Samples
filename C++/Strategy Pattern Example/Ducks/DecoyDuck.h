#pragma once
#include"Duck.h"

class DecoyDuck :public Duck
{
public:
	explicit DecoyDuck ();
	virtual void callQuackBehavior () const override;
	virtual void callFlyBehavior () const override;
	virtual void display () const override;
};
