#include<iostream>
#include"Subject.h"
#include"WeatherData.h"
#include"ForcastDisplay.h"
#include"CurrentConditionsDisplay.h"
#include"StatisticsDisplay.h"

using namespace std;

int main ()
{
	WeatherData* weatherdata = new WeatherData ();
	CurrentConditionsDisplay* currentDisplay = new CurrentConditionsDisplay( weatherdata, "Current Conditions Display" );
	StatisticsDisplay* statDisplay = new StatisticsDisplay( weatherdata, "Statistics Display" );
	ForcastDisplay* forcastDisplay = new ForcastDisplay( weatherdata, "Forcast Display" );
	cout << endl;
	cout << endl << "Displaying observers..." << endl << endl;
	weatherdata->setMeasurements ( 65, 40, 10 );
	weatherdata->removeObserver ( forcastDisplay );
	cout << endl << "Displaying observers..." << endl << endl;
	weatherdata->setMeasurements ( 75, 50, 20 );
	weatherdata->removeObserver ( statDisplay );
	cout << endl << "Displaying observers..." << endl << endl;
	weatherdata->setMeasurements ( 85, 80, 30 );
	cout << endl << "Displaying observers..." << endl << endl;
	weatherdata->setMeasurements ( 55, 20, 20 );
	cout << endl << "Displaying observers..." << endl << endl;
	weatherdata->setMeasurements ( 65, 30, 10 );

	delete weatherdata, currentDisplay, statDisplay, forcastDisplay;


	system("pause");
}