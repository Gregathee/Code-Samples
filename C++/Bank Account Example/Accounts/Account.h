#pragma once

#include<iostream>
#include<string>
#include"DollarAmount.h"

class Account
{
private:
	std::string mFirstName;
	std::string mLastName;
	unsigned int mAccountNumber; 
	DollarAmount mBalance;

protected:
	static unsigned sAccountTally;

public:
	Account ();
	Account ( std::string fName, std::string lName,  DollarAmount bal );

	int getAccountNumber () const;

	void setFirstName ( std::string fName );
	std::string getFirstName () const;

	void setLastName ( std::string lName );
	std::string getLastName () const;

	void setBalance ( DollarAmount bal );
	DollarAmount getBalance () const;

	void deposit ( DollarAmount dep);
	virtual void withdraw ( DollarAmount );

	virtual void display () const;

	virtual void setInterest (double rate);
	
};
