#include "ForcastDisplay.h"
#include<iostream>

ForcastDisplay::ForcastDisplay ( Subject * weatherData, std::string name ): Observer(name) { weatherData->registerObserver ( this ); }

void ForcastDisplay::display ()
{
	std::cout << "Current forcast:\n" <<
		"Temperaure: How the hell should I know\n" <<
		"Humidity: No clue\n" <<
		"Pressure: Your guess is as good as mine\n\n";
}

void ForcastDisplay::update ( float temp, float humidity, float preassure ) { display (); }
