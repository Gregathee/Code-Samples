#pragma once
#include"FlyingBehavior.h"

class RocketPower: public FlyingBehavior
{
public:
	RocketPower () {};
	void fly () const;
};