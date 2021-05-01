//Driver class to demonstrated a decorate pattern example 

//User is prompted to select a beverage from a list and then is able to decorate it with toppings

#include<iostream>
#include"DarkRoast.h"
#include"Decaf.h"
#include"Espresso.h"
#include"HouseBlend.h"
#include"Milk.h"
#include"Soy.h"
#include"Whip.h"
#include"Mocha.h"

using std::cout;
using std::endl;
using std::cin;

void InitialBeveragePrompt ( int& selection, Beverage*& beverage, bool& hasQuit, bool& selectionIsGood );
void InitialCondimentBeveragePrompt ( int& selection, bool& hasQuit, bool& selectionIsGood );
void AdditionalCondimentPrompt ( int& selection, bool& hasQuit, bool& selectionIsGood );
void CondimentPrompt ( int& selection, Beverage*& beverage, bool& hasQuit, bool& selectionIsGood );

int main ()
{
	bool hasQuit = false;
	bool selectionIsGood = false;
	int selection;
	Beverage* beverage = nullptr;

	//Prompt user to select a beverage
	while ( !hasQuit && !selectionIsGood) {  InitialBeveragePrompt ( selection, beverage, hasQuit, selectionIsGood ); }
	
	//Print results of input unless user chose to quit
	if ( !hasQuit )
	{
		cout << endl;
		beverage->display( );
		cout << endl;
	}
	selectionIsGood = false;

	//Prompt user to add a condiment
	while ( !hasQuit && !selectionIsGood) { InitialCondimentBeveragePrompt ( selection, hasQuit, selectionIsGood ); }

	selectionIsGood = false;
	cout << endl;

	//Prompt user to to select a condiment
	while ( !hasQuit && !selectionIsGood ) { CondimentPrompt ( selection, beverage, hasQuit, selectionIsGood ); }

	system ( "pause" );
}

void InitialBeveragePrompt (int &selection, Beverage* &beverage, bool &hasQuit, bool &selectionIsGood )
{
	selectionIsGood = true;
	//Prompt user
	cout << "Enter the number of the beverage you would like to order?" <<
		"\n1. House Blend $" << HouseBlend::price <<
		"\n2. Dark Roast $" << DarkRoast::price <<
		"\n3. Decaf $" << Decaf::price <<
		"\n4. Espresso $" << Espresso::price <<
		"\n5. Nevermind" <<
		"\n\n Selection: ";

	//Collect input
	cin >> selection;
	cout << endl;

	//Analyze input, then quit, prompt error or create a beverage
	switch ( selection )
	{
	case 1: beverage = new HouseBlend (); break;
	case 2: beverage = new DarkRoast (); break;
	case 3: beverage = new Decaf (); break;
	case 4: beverage = new Espresso (); break;
	case 5: hasQuit = true; break;
	default:
		cout << "Please selected a number 1-5\n\n";
		selectionIsGood = false;
		break;
	}
}

void InitialCondimentBeveragePrompt ( int& selection, bool& hasQuit, bool& selectionIsGood )
{
	selectionIsGood = true;
	//Prompt user
	cout << "Would you like to add a condiment?\n" <<
		"1. yes\n" <<
		"2. no\n" <<
		"\n\nSelection: ";

	//Collect input
	cin >> selection;
	cout << endl;

	//Analyze input, then quit, prompt error or continue to condement menu
	switch ( selection )
	{
	case 1: break;
	case 2:
		hasQuit = true;
		cout << "Enjoy your beverage\n";
		break;
	default:
		selectionIsGood = false;
		cout << "Please select 1 or 2\n";
		break;
	}
}

void AdditionalCondimentPrompt ( int& selection, bool& hasQuit, bool& selectionIsGood )
{
	selectionIsGood = true;

	//Prompt user
	cout << "\n\nWould you like to add a another condiment?\n" <<
		"1. yes\n" <<
		"2. no\n" <<
		"\n\nSelection: ";

	cin >> selection;
	cout << endl;

	//Analyze input, then quit, prompt error or continue
	switch ( selection )
	{
	case 1: break;
	case 2:
		hasQuit = true;
		cout << "Enjoy your beverage\n";
		break;
	default:
		selectionIsGood = false;
		cout << "Please select 1 or 2\n";
		break;
	}
}

void CondimentPrompt ( int& selection, Beverage*& beverage, bool& hasQuit, bool& selectionIsGood )
{
	selectionIsGood = true;

	cout << "Enter the number of the condiment you would like to add." <<
		"\n1. Milk $" << Milk::price << 
		"\n2. Soy $" << Soy::price <<
		"\n3. Mocha $" << Mocha::price <<
		"\n4. Whip $" << Whip::price <<
		"\n5. Never Mind" <<
		"\n\nSelection: ";
	cin >> selection;
	cout << endl;

	//Analyze input, then quit, prompt error or add a condiment
	switch ( selection )
	{
	case 1: beverage = new Milk ( beverage  ); break;
	case 2: beverage = new Soy (beverage  ); break;
	case 3: beverage = new Mocha ( beverage  ); break;
	case 4: beverage = new Whip (  beverage  ); break;
	case 5:
		hasQuit = true;
		cout << "Enjoy your beverage\n";
		break;
	default:
		cout << "Please enter a number 1-5\n";
		selectionIsGood = false;
		break;
	}

	selectionIsGood = false;
	beverage->display ( );

	//Prompt user for another condiment then repeat or quit
	while ( !hasQuit && !selectionIsGood ) { AdditionalCondimentPrompt ( selection, hasQuit, selectionIsGood ); }

	selectionIsGood = false;
}
