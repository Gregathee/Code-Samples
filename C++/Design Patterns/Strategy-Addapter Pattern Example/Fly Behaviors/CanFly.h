#pragma once
#include"FlyingBehavior.h"

class CanFly : public FlyingBehavior
{
public:
	CanFly () {};
	void fly () const;
};