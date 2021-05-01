#include "PremiumChecking.h"

PremiumChecking::PremiumChecking ():Account(){}

PremiumChecking::PremiumChecking ( std::string fName, std::string lName, DollarAmount bal )
	:Account(fName, lName, bal) { chargeMonthlyFee (); }

void PremiumChecking::setMonthlyFee ( DollarAmount fee ) { mFee = fee; }

DollarAmount PremiumChecking::getMonthlyFee () { return mFee; }

void PremiumChecking::chargeMonthlyFee () { setBalance(getBalance () - mFee); }

void PremiumChecking::display () const
{
	Account::display ();

	std::cout << "Account type: Premium Checking\n" 
		<< "Monthly fee: " << mFee << std::endl;
}
