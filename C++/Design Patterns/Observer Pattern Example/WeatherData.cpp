#include "WeatherData.h"
#include "Observer.h"
#include<iostream>

void WeatherData::registerObserver ( Observer* observer ) 
{
	std::cout << observer->name << " registers as observer" << std::endl;
	observers.push_back ( observer ); 
}

void WeatherData::removeObserver ( Observer* observer ) 
{
	std::cout << observer->name << " unregisters as observer" << std::endl;
	observers.remove ( observer ); 
}

void WeatherData::notifyObservers () { for ( Observer* ob : observers ) { ob->update (temperature, humidity, preassure); } }

float WeatherData::getTemperature () { return temperature; }

float WeatherData::getHumidity () { return humidity; }

float WeatherData::getPressure () { return preassure; }

void WeatherData::measurementsChanged () { notifyObservers (); }

void WeatherData::setMeasurements ( float temperature, float humidity, float preassure )
{
	this->temperature = temperature;
	this->humidity = humidity;
	this->preassure = preassure;
	measurementsChanged ();
}
