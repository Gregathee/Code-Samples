#pragma once
#include "Account.h"
#include<map>


enum class accountType { savings, checking, premiumChecking };

static class AccountFactory
{
public:
	static void accountSuccessDisplay (const bool &success );
	static void createAccounts ( const int  &howMany, const accountType &type, std::map<int, Account*> &AccountPtrs );
	static void displayAccounts ( std::map<int, Account*> accountPtrs );

};