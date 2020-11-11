#pragma once
#include"QuackingBehavior.h"

class Quack : public QuackingBehavior
{
public:
	Quack () {};
	void quack () const;
};