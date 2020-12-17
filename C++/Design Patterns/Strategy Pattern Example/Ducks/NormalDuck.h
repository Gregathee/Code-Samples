#pragma once
#include"Duck.h"


class NormalDuck : public Duck
{
public:
	NormalDuck ();
	virtual void callQuackBehavior () const override;
	virtual void callFlyBehavior () const override;
	virtual void display () const override;
};