#include "Beverage.h"
#include <iostream>

using std::cout;
using std::endl;

std::string Beverage::getDescription ( ) { return description; }

void Beverage::display () { cout << endl << description << "\nThat'll be $" << cost ( 0 ); }

bool StringContains ( std::string target, std::string onString )
{
	//Check for trivial cases
	if ( target.length () == 0 ) {  return true; }
	if ( target.length () > onString.length () ) { return false; }
	int i;
	bool wordFound = false;

	//find the index of begining of replacee in onString
	for ( i = 0; i < onString.length () && !wordFound; i++ )
	{
		//find first letter of replacee then check if the characters following form the word
		if ( onString[ i ] == target[ 0 ] )
		{
			wordFound = true;
			for ( int j = 1; j < target.length () && i+j < onString.length(); j++ ) 
			{
				if ( target[ j ] != onString[ i + j ] ) { wordFound = false; break; } 
			}
		}
	}
	return wordFound;
}

void Beverage::Replace ( const std::string _replacee, const std::string replacer, std::string& _onString, int skip )
{
	int skipped = 0;
	std::string replacee = _replacee;
	std::string onString = _onString;
	//Check for valid input
	if (!StringContains(replacee, onString) || replacee.length () == 0 ) { return; }
	int i;
	bool wordFound = false;
	do
	{
		wordFound = false;
		//find the index of begining of replacee in onString
		for ( i = 0; i < onString.length () && !wordFound; i++ )
		{
			//find first letter of replacee then check if the characters following form the word
			if ( onString[ i ] == replacee[ 0 ] )
			{
				wordFound = true;
				for ( int j = 1; j < replacee.length (); j++ ) { if ( replacee[ j ] != onString[ i + j ] ) { wordFound = false; break; } }
			}
		}
		++skipped;
	}while ( skipped < skip );
	if ( wordFound ) { _onString = onString.replace ( i-2, replacee.length ()+2, replacer ); }
}



void Beverage::appendDescription ( Beverage*& beverage )
{
	std::string originalDescription = description;
	std::string newDescription = beverage->getDescription ();

	//Format string puncuation
	if ( StringContains ( " and", newDescription ) )
	{
		if ( StringContains ( ",", newDescription ) )
		{
			int commaCount = 0;
			char comma = ',';
			for (int i = 0; i < newDescription.length(); i++) { if ( newDescription[ i ] == comma ) { commaCount++; } }
			Beverage::Replace ( "and", ", ", newDescription , commaCount -1);
		}
		else {  Beverage::Replace ( "and", ", ", newDescription ); }
		description = newDescription + " and " + originalDescription;
	}
	else if ( StringContains ( " with", newDescription ) ) {  description = newDescription + " and " + originalDescription; }
	else {description = newDescription + " with " + originalDescription; }
}
