#pragma once
#include"QuackingBehavior.h"

class Squeak: public QuackingBehavior
{
public:
	Squeak () {};
	virtual void quack () const override;
};