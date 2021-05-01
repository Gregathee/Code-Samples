#pragma once
#include "Command.h"
#include<vector>

class FanSpeed: public Command
{
private:
	void PrintSpeed ();
	const std::vector<int> speedSettings{ 0, 1, 2, 3 };
	int speed;
public:
	virtual void Execute () override;
	virtual void Undo () override;
	virtual std::string Name () override;
};