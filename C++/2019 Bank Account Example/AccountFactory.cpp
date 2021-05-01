#include "AccountFactory.h"
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

void AddRandomAttributesToAccount ( Account*& account, default_random_engine& engine )
{
	vector<string> firstNames{ "John", "Abraham", "George", "Phill", "Jain" };
	vector<string> lastNames{ "Washington", "Lincoln", "Smith", "Doe", "Collins" };
	DollarAmount ranBal;

	uniform_int_distribution<int64_t> randomBalance{ 0, 1000 };
	uniform_int_distribution<int64_t> randomNames{ 0, 4 };

	ranBal.setAmount ( randomBalance ( engine ) );
	account->setBalance ( ranBal );
	account->setFirstName ( firstNames[ randomNames ( engine ) ] );
	account->setLastName ( lastNames[ randomNames ( engine ) ] );
}

void CreateAccount ( Account* account, default_random_engine& engine, pair < map<int, Account*>::iterator, bool >& accountMapPair, map<int, Account*>& AccountPtrs )
{
	AddRandomAttributesToAccount ( account, engine );
	accountMapPair = AccountPtrs.insert ( make_pair ( account->getAccountNumber (), account ) );
	AccountFactory::accountSuccessDisplay ( accountMapPair.second );
}

//Create accounts with random names and intrest rates and balances
void AccountFactory::createAccounts ( const int &howMany, const accountType &type, map<int, Account*> &AccountPtrs )
{
	vector<string> firstNames{ "John", "Abraham", "George", "Phill", "Jain" };
	vector<string> lastNames{ "Washington", "Lincoln", "Smith", "Doe", "Collins" };
	
	pair < map<int, Account*>::iterator, bool > accountMapPair;

	default_random_engine engine{ static_cast< unsigned int >( time ( 0 ) ) };
	normal_distribution<double> randomInterest{ 5.0, 1.0 };

	bool success;

	cout << "Creating Accounts..." << endl;

	switch ( type )
	{
	case accountType::checking:
		for ( unsigned int i = 0; i < howMany; i++ ) { CreateAccount ( new Checking (), engine, accountMapPair, AccountPtrs ); }
		break;
	case accountType::premiumChecking:
		for ( unsigned int i = 0; i < howMany; i++ ) { CreateAccount ( new PremiumChecking (), engine, accountMapPair, AccountPtrs ); }
		break;
	case accountType::savings:
		for ( unsigned int i = 0; i < howMany; i++ )
		{
			Account* account = new Savings ();
			double randomRate = randomInterest ( engine );
			account->setInterest ( randomRate );
			CreateAccount ( account, engine, accountMapPair, AccountPtrs );
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
