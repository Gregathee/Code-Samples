#include "Checking.h"

Checking::Checking ():Account(){}

Checking::Checking ( std::string fName, std::string lName, DollarAmount bal )
	:Account(fName, lName, bal){}

void Checking::withdraw ( DollarAmount withdraw )
{
	if ( Account::getBalance () - withdraw > 0 ) { Account::withdraw ( withdraw ); }
	else { chargePenalty (); }
}

void Checking::chargePenalty ()
{
	DollarAmount tempDollar;
	
	if ( mOverdrawnCount > 3 )
	{
		setBalance ( getBalance () - underThreePenalty );
		tempDollar = underThreePenalty;
	}
	else
	{
		setBalance ( getBalance () - ThreeOrOverPenalty );
		tempDollar = ThreeOrOverPenalty;
	}

	++mOverdrawnCount;
	std::cout << "You have " << mOverdrawnCount << " Overdrafts on your Account. " << std::endl <<
		"Your account has been charged " << tempDollar << "." << std::endl <<
		"Your current balance is " << getBalance ();
}

void Checking::display () const
{
	Account::display ();
	
	std::cout << "Account type: Checking\n" 
		<< "Overdrafts: " << mOverdrawnCount << std::endl;
}


