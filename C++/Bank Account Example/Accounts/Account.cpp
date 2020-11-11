#include "Account.h"

unsigned int Account::sAccountTally{ 0 };

Account::Account ()
{
	++sAccountTally;
	mAccountNumber = sAccountTally;
}

Account::Account ( std::string fName, std::string lName,  DollarAmount bal )
	:mFirstName{ fName }, mLastName{ lName }
{
	if ( bal > 0 ) { mBalance = bal; }

	++sAccountTally;
	mAccountNumber= sAccountTally;
}

int Account::getAccountNumber () const { return mAccountNumber; }

void Account::setFirstName ( std::string fName ) { mFirstName = fName; }

std::string Account::getFirstName () const { return mFirstName; }

void Account::setLastName ( std::string lName ) { mLastName = lName; }

std::string Account::getLastName () const { return mLastName; }

void Account::setBalance ( DollarAmount bal ) { mBalance = bal; }

DollarAmount Account::getBalance () const { return mBalance; }

void Account::deposit ( DollarAmount dep)
{
	if ( dep > 0 ) { mBalance += dep; }
	else { std::cout << "Deposit amount must be positive number." << std::endl; }
}

void Account::withdraw ( DollarAmount withdraw )
{
	if ( withdraw > 0 )
	{
		if ( mBalance - withdraw > 0 ) { mBalance -= withdraw; }
		else { std::cout << "Insufficient funds." << std::endl; }
	}
	else { std::cout << "Withdraw amount must be positive number." << std::endl; }
}

void Account::display () const
{
	std::cout << "\nAccount Number: " << mAccountNumber <<
		"\nFirst Name: " << mFirstName <<
		"\nLast Name: " << mLastName << 
		"\nBalance: " << mBalance << std::endl;
}

void Account::setInterest (double rate){}
