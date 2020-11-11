#pragma once
#include "DisplayElement.h"
#include "Observer.h"
#include "Subject.h"

class CurrentConditionsDisplay: public Observer, DisplayElement
{
private:
	float temperature;
	float humidity;
	float preassure;
public:
	CurrentConditionsDisplay ( Subject* weatherData, std::string name );
	virtual void display ();
	virtual void update ( float temp, float humidity, float preassure );
};
