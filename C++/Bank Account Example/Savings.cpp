#include "Savings.h"

Savings::Savings () :Account () {}

Savings::Savings ( std::string fName, std::string lName, DollarAmount bal, double rate )
	:Account(fName, lName, bal)
{
	mInterestRate = rate;
}

void Savings::setInterest ( double rate )
{
	mInterestRate = rate;
}

double Savings::getIntrest () const
{
	return mInterestRate;
}

void Savings::display () const
{
	Account::display ();
	
	std::cout<< "Account type: Savings\n" 
		<< "Interest rate: " << mInterestRate << std::endl;
}
