#pragma once

#include"Account.h"


class PremiumChecking : public Account
{
private:
	DollarAmount mFee{ 5 };
public:
	PremiumChecking ();
	PremiumChecking ( std::string fName, std::string lName, DollarAmount bal );

	void setMonthlyFee ( DollarAmount fee );
	DollarAmount getMonthlyFee ();
	void chargeMonthlyFee ();

	void display () const override;
};