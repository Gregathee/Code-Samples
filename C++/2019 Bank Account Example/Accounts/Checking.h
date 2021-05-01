#pragma once


#include"Account.h"


class Checking: public Account
{
private:
	unsigned int mOverdrawnCount{ 0 };
	DollarAmount const underThreePenalty{ 10 };
	DollarAmount const ThreeOrOverPenalty{ 30 };

public:
	Checking ();
	Checking ( std::string fName, std::string lName, DollarAmount bal );
	void withdraw (DollarAmount withdraw) override;

	void chargePenalty ();
	
	void display () const override;
};