#include "Checking.h"
#include "PremiumChecking.h"
#include "Savings.h"
#include "DollarAmount.h"
#include<iostream>
#include<string>
#include<vector>
#include<ctime>
#include<random>
#include<map>
#include<utility>
#include"AccountHelper.h"

using std::cout;
using std::cin;
using std::endl;
using std::map;
using std::numeric_limits;
using std::streamsize;

void MainMenu ( unsigned& selection);
void CreateAccounts ( unsigned& selection, unsigned& numberOfAccounts, map<int, Account*>& accountPtrs, map<int, Account*>::iterator& selectedAccount, bool& isQuitSelected );

int main ()
{
	map<int, Account*> accountPtrs;
	unsigned int selection = 0;
	unsigned int numberOfAccounts;
	bool isQuitSelected{ false };

	cout << "Press enter to begin..." << endl;
	while (!isQuitSelected)
	{
		DollarAmount balance{ 0 };
		double interestRate{ 0 };

		map<int, Account*>::iterator selectedAccount;
		do { MainMenu (selection); } while (cin.fail());

		if (selection < 4 && selection > 0)
		{
			cout << "How many accounts would you like created?\n";
			cin >> numberOfAccounts;
			cout << endl << endl;;
		}
		CreateAccounts (selection, numberOfAccounts, accountPtrs, selectedAccount, isQuitSelected);
	}
	system ( "pause" );
	return 0;
}

void MainMenu ( unsigned& selection)
{
	cin.clear ();
	cin.ignore ( numeric_limits<streamsize>::max (), '\n' );

	cout << "MENU:\n" <<
		"1. Create Checking accounts\n" <<
		"2. Create Premium accounts\n" <<
		"3. Create Savings accounts\n" <<
		"4. Delete single account\n" <<
		"5. Display single account\n" <<
		"6. Display all accounts\n"
		"7. Quit\n\n" <<
		"Selection :";

	cin >> selection;
	cout << endl;

	if ( cin.fail () ) { cout << "Invalid input.\n\n"; }
}

void CreateAccounts ( unsigned& selection, unsigned& numberOfAccounts, map<int, Account*>& accountPtrs, map<int, Account*>::iterator& selectedAccount, bool& isQuitSelected)
{
	switch ( selection )
	{
	case 1: AccountFactory::createAccounts ( numberOfAccounts, accountType::checking, accountPtrs ); break;
	case 2: AccountFactory::createAccounts ( numberOfAccounts, accountType::premiumChecking, accountPtrs ); break;
	case 3: AccountFactory::createAccounts ( numberOfAccounts, accountType::savings, accountPtrs ); break;
	case 4:
		cout << "Enter the account number of the account you wish to delete.\n\n";
		cin >> numberOfAccounts;

		accountPtrs.erase ( numberOfAccounts );
		break;
	case 5:
		cout << "Enter the account number you wish to be displayed\n\n";
		cin >> numberOfAccounts;
		selectedAccount = accountPtrs.find ( numberOfAccounts );

		if ( selectedAccount != accountPtrs.cend () ) { accountPtrs.at ( numberOfAccounts )->display (); }
		else { cout << "\nAccount not found.\n"; }

		cout << endl;
		break;
	case 6: AccountFactory::displayAccounts ( accountPtrs ); break;
	case 7: isQuitSelected = true; break;
	default: cout << "\n\nInvalid input please input a number 1-7.\n\n"; break;
	}
}
