#pragma once
#include "Duck.h"
#include "Goose.h"

class GooseToDuck: public Duck
{
public:
	Goose* goose;
	void callQuackBehavior () const override;
	void callFlyBehavior () const override;
	void display () const override;
	void swim () const override;
	GooseToDuck ( Goose* goose );
};