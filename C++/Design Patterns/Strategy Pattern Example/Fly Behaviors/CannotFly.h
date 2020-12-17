#pragma once
#include"FlyingBehavior.h"


class CannotFly : public FlyingBehavior
{
public:
	CannotFly () {};
	void fly () const;
};
