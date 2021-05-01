#include<iostream>
#include<string>
#include"ChicagoPizzaPlace.h"
#include"NYPizzaPlace.h"

using std::cout;
using std::cin;
using std::endl;
using std::string;

void StartOrder (int& selection, PizzaPlace*& pizzaPlace, bool& exit, bool& selectionIsGood);
void SpecifyOrder ( int& selection, PizzaPlace*& pizzaPlace, bool& exit, bool& selectionIsGood );
void Reorder ( int& selection, bool& exit, bool& selectionIsGood );

int main ()
{
	bool exit = false;
	bool selectionIsGood = false;
	int selection = 0;  
	PizzaPlace* pizzaPlace = nullptr;
	while(!exit)
	{
		StartOrder (selection, pizzaPlace, exit, selectionIsGood);
		SpecifyOrder (selection, pizzaPlace, exit, selectionIsGood);
		Reorder (selection, exit, selectionIsGood);
	}
}

void StartOrder ( int& selection, PizzaPlace*& pizzaPlace, bool& exit, bool& selectionIsGood )
{
	selectionIsGood = false;
	while ( !exit && !selectionIsGood )
	{
		selectionIsGood = true;
		int selection;
		cout << "Please select a pizza." << endl <<
			"1. New York Style Pizza" << endl <<
			"2. Chicago Style Pizza" << endl <<
			"3. Quit" << endl << endl <<
			"Selection: ";

		cin >> selection;
		cout << endl;

		switch ( selection )
		{
		case 1: pizzaPlace = new NYPizzaPlace (); break;
		case 2: pizzaPlace = new ChicagoPizzaPlace (); break;
		case 3: exit = true;  break;
		default:
			cout << "Please select a number 1-3" << endl << endl;
			selectionIsGood = false;
			break;
		}
	}
}

void SpecifyOrder ( int& selection, PizzaPlace*& pizzaPlace, bool& exit, bool& selectionIsGood )
{
	selectionIsGood = false;
	while ( !exit && !selectionIsGood )
	{
		selectionIsGood = true;
		int selection;
		cout << "Please select a pizza type." << endl <<
			"1. Cheese Pizza" << endl <<
			"2. Classic Pizza" << endl <<
			"3. Quit" << endl << endl <<
			"Selection: ";

		cin >> selection;
		cout << endl;

		switch ( selection )
		{
		case 1: pizzaPlace->OrderPizza ( "Cheese" ); break;
		case 2: pizzaPlace->OrderPizza ( "Classic" ); break;
		case 3: exit = true;  break;
		default:
			cout << "Please select a number 1-3" << endl << endl;
			selectionIsGood = false;
			break;
		}
	}
}

void Reorder ( int& selection, bool& exit, bool& selectionIsGood )
{
	selectionIsGood = false;
	while ( !exit && !selectionIsGood )
	{
		selectionIsGood = true;
		int selection;
		cout << endl << "Would you like to reorder?" << endl <<
			"1. Yes" << endl <<
			"2. No" << endl << endl <<
			"Selection: ";

		cin >> selection;
		cout << endl;

		switch ( selection )
		{
		case 1: break;
		case 2: exit = true; break;
		default:
			cout << "Please select a number 1-2" << endl << endl;
			selectionIsGood = false;
			break;
		}
	}
}