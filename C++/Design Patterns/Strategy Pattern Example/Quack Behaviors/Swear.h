
#pragma once
#include"QuackingBehavior.h"

class Swear: public QuackingBehavior
{
public:
	Swear () {};
	void quack () const;
};
