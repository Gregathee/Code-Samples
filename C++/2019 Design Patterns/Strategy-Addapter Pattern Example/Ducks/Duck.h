#pragma once
#include"FlyingBehavior.h"
#include"QuackingBehavior.h"

class Duck
{
protected:
	FlyingBehavior* flyingBehavior;
	QuackingBehavior* quackingBehavior;

public:
	virtual void callQuackBehavior () const = 0;
	virtual void callFlyBehavior () const = 0 ;
	virtual void display() const = 0;
	
	virtual void setQuackBehavior (QuackingBehavior*& quackIn);
	virtual void setFlyBehavior (FlyingBehavior*& flyIn);
	void swim () const;

};