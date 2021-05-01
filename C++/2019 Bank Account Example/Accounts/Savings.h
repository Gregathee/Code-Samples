#pragma once

#include"Account.h"


class Savings: public Account
{
private:
	double mInterestRate;

public:
	Savings ();

	Savings ( std::string fName, std::string lName, DollarAmount bal, double rate );

	void setInterest ( double rate ) override;
	double getIntrest () const;

	void display ()const override;


};
