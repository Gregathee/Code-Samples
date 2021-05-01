#pragma once
#include "Command.h"

class AlarmOnOff: public Command
{
private:
	bool isOn = false;
public:
	virtual void Execute () override;
	virtual void Undo () override;
	virtual std::string Name () override;
};