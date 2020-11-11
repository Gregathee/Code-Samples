#pragma once
#include<string>
#include<memory>

class Beverage
{
public:
	virtual std::string getDescription ( );
	virtual void display ();
	virtual float cost (float addedCost) = 0;
	static void Replace ( const std::string _replacee, const std::string replacer, std::string& _onString, int skip = 0 );
	void appendDescription ( Beverage*& beverage );

protected:
	std::string description = "Unnamed";
};
