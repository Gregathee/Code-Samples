#pragma once

#include<iostream>
#include <ostream>
#include <string>
#include <cmath> 

class DollarAmount
{
private:
	int64_t amount{ 0 }; // dollar amount in pennies

public:
	DollarAmount ();
	DollarAmount ( double value );

	friend DollarAmount operator+=( DollarAmount& left, const DollarAmount& right );
	friend DollarAmount operator-=( DollarAmount& left, const DollarAmount& right );
	friend bool operator<( const DollarAmount& left, const DollarAmount& right );
	friend bool operator<=( const DollarAmount& left, const DollarAmount& right );
	friend bool operator>( const DollarAmount& left, const DollarAmount& right );
	friend bool operator>=( const DollarAmount& left, const DollarAmount& right );
	friend bool operator==( const DollarAmount& left, const DollarAmount& right );
	friend bool operator<( const DollarAmount& left, const DollarAmount& right );
	friend bool operator<=( const DollarAmount& left, const int& right );
	friend bool operator>( const DollarAmount& left, const int& right );
	friend bool operator>=( const DollarAmount& left, const int& right );
	friend bool operator==( const DollarAmount& left, const int& right );
	friend std::ostream& operator<<( std::ostream &os, const DollarAmount &amount );
	
	void setAmount ( double value );
	void operator=( int right );
	DollarAmount operator+ ( DollarAmount right );
	DollarAmount operator- ( DollarAmount right );
	void addInterest ( int rate, int divisor );
	void addInterest ( double rate );
	std::string toString () const;
	int64_t toPennies ();


};