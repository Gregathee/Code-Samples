#pragma once
#include"Subject.h"
#include"Observer.h"
#include<list>

class WeatherData: public Subject
{
private:
	std::list<Observer*> observers;
	float temperature;
	float humidity;
	float preassure;
public:
	virtual void registerObserver ( Observer* observer );;
	virtual void removeObserver ( Observer* observer );
	virtual void notifyObservers ();

	float getTemperature ();
	float getHumidity ();
	float getPressure ();
	void measurementsChanged ();
	void setMeasurements ( float temp, float humidity, float preassure );

};