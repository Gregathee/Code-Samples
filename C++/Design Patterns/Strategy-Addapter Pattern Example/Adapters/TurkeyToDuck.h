#pragma once
#include"Duck.h"
#include"Turkey.h"

class TurkeyToDuck: public Duck
{
public:
	Turkey* turkey;
	void callQuackBehavior () const override;
	void callFlyBehavior () const override;
	void display () const override;
	void swim () const override;
	TurkeyToDuck ( Turkey* turkey ); 
};