#pragma once
#include<string>

class Observer
{
public:
	std::string name;
	Observer (std::string name);
	virtual void update (float temp, float humidity, float preassure) = 0;
};
