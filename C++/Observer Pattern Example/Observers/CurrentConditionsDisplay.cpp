#include "CurrentConditionsDisplay.h"
#include<iostream>

CurrentConditionsDisplay::CurrentConditionsDisplay ( Subject* weatherData, std::string name ) : Observer(name)
{
	weatherData->registerObserver ( this );
}

void CurrentConditionsDisplay::display ()
{
	std::cout << "Current Conditions:\nTemperature: " << temperature << "\nHumidity: " << humidity << "\nPressure: " << preassure << std::endl << std::endl;
}

void CurrentConditionsDisplay::update ( float temperature, float humidity, float preassure )
{
	this->temperature = temperature;
	this->humidity = humidity;
	this->preassure = preassure;
	display ();
}
