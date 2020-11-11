#pragma once
#include "DisplayElement.h"
#include "Observer.h"
#include"Subject.h"

class ForcastDisplay: public Observer, DisplayElement
{
public:
	ForcastDisplay ( Subject* weatherData, std::string name );
	virtual void display ();
	virtual void update ( float temp, float humidity, float preassure );
};