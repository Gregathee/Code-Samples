#include "AccountHelper.h"
#include <vector>
#include<string>
#include<ctime>
#include<random>
#include<map>
#include"Checking.h"
#include"Savings.h"
#include"PremiumChecking.h"
#include<utility>

using std::cout;
using std::endl;
using std::vector;
using std::string;
using std::pair;
using std::map;
using std::default_random_engine;
using std::uniform_int_distribution;
using std::normal_distribution;
using std::make_pair;

void AccountFactory::accountSuccessDisplay (const bool &success ) { success ? cout << "Account created succesfully.\n\n" : cout << "Account could not be created.\n"; }

void AccountFactory::createAccounts ( const int &howMany, const accountType &type, map<int, Account*> &AccountPtrs )
{
	vector<string> firstNames{ "John", "Abraham", "George", "Phill", "Jain" };
	vector<string> lastNames{ "Washington", "Lincoln", "Smith", "Doe", "Collins" };
	
	pair < map<int, Account*>::iterator, bool > accountMapPair;

	default_random_engine engine{ static_cast< unsigned int >( time ( 0 ) ) };
	uniform_int_distribution<int64_t> randomBalance{ 0, 1000 };
	uniform_int_distribution<int64_t> randomNames{ 0, 4 };
	normal_distribution<double> randomInterest{ 5.0, 1.0 };

	bool success;

	cout << "Creating Accounts..." << endl;

	switch ( type )
	{
	case accountType::checking:
		for ( unsigned int i = 0; i < howMany; i++ )
		{
			Account* account = new Checking ();
			DollarAmount ranBal;
			ranBal.setAmount ( randomBalance ( engine ) );

			account->setBalance ( ranBal );
			account->setFirstName ( firstNames[ randomNames ( engine ) ] );
			account->setLastName ( lastNames[ randomNames ( engine ) ] );
			accountMapPair = AccountPtrs.insert ( make_pair ( account->getAccountNumber (), account ) );
			accountSuccessDisplay ( accountMapPair.second );
		}
		break;
	case accountType::premiumChecking:
		for ( unsigned int i = 0; i < howMany; i++ )
		{
			Account* account = new PremiumChecking ();
			DollarAmount ranBal;
			ranBal.setAmount ( randomBalance ( engine ) );

			account->setBalance ( ranBal );
			account->setFirstName ( firstNames[ randomNames ( engine ) ] );
			account->setLastName ( lastNames[ randomNames ( engine ) ] );
			accountMapPair = AccountPtrs.insert (make_pair ( account->getAccountNumber (), account ) );
			accountSuccessDisplay ( accountMapPair.second );
		}
		break;
	case accountType::savings:
		for ( unsigned int i = 0; i < howMany; i++ )
		{
			Account* account = new Savings ();
			DollarAmount ranBal;
			ranBal.setAmount ( randomBalance ( engine ) );
			double randomRate = randomInterest ( engine );

			account->setBalance ( ranBal );
			account->setFirstName ( firstNames[ randomNames ( engine ) ] );
			account->setLastName ( lastNames[ randomNames ( engine ) ] );
			account->setInterest ( randomRate );
			accountMapPair = AccountPtrs.insert ( make_pair ( account->getAccountNumber (), account ) );
			accountSuccessDisplay ( accountMapPair.second );
		}
		break;
	}
}

void AccountFactory::displayAccounts ( map<int, Account*> accountPtrs )
{
	int count = 0;

	for ( auto mapItem : accountPtrs )
	{
		mapItem.second->display ();
		cout << endl;
		++count;
	}
	cout << "Displaying " << count << " accounts.\n\n";
}
