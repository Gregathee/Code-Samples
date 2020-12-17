#include "StatisticsDisplay.h"
#include<iostream>

float StatisticsDisplay::calculateAverage ( std::vector<float> stats )
{
	float average = 0;
	for ( int i = 0; i < stats.size(); ++i ) { average += stats[ i ]; }
	return (average /= stats.size ());
}

StatisticsDisplay::StatisticsDisplay ( Subject* weatherData, std::string name ) : Observer ( name ) {  weatherData->registerObserver ( this ); }

void StatisticsDisplay::display ()
{
	std::cout << "Weather statistics: " <<
		"\nAverage temperature: " << calculateAverage(temperature) << 
		"\nAverage humidity: " << calculateAverage ( humidity ) <<
		"\nAverage pressure: " << calculateAverage ( pressure ) << std::endl << std::endl;
}

void StatisticsDisplay::update ( float temperature, float humidity, float preassure )
{
	this->temperature.push_back ( temperature );
	this->humidity.push_back ( humidity );
	this->pressure.push_back ( preassure );
	display();
}
