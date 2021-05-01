#pragma once
#include "DisplayElement.h"
#include "Observer.h"
#include "Subject.h"
#include <vector>
#include <string>

class StatisticsDisplay: public Observer, DisplayElement
{
private:
	std::vector<float> temperature;
	std::vector<float> humidity;
	std::vector<float> pressure;
	

	float calculateAverage ( std::vector<float> stats );
public:
	StatisticsDisplay ( Subject* weatherData, std::string name );
	virtual void display ();
	virtual void update ( float temp, float humidity, float preassure );
};