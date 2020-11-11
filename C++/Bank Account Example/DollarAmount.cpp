#include "DollarAmount.h"

DollarAmount::DollarAmount () :amount{ 0 } {};


DollarAmount::DollarAmount ( double value ) : amount{ ( int64_t) ( value * 100 ) } {};

DollarAmount operator+=( DollarAmount& left, const DollarAmount & right )
{
	left = left + right;
	return left;
}

DollarAmount operator-=( DollarAmount& left, const DollarAmount& right )
{
	left = left - right;
	return left;
}

bool operator<( const DollarAmount & left, const DollarAmount & right )
{
	if ( left.amount < right.amount )
		return true;
	return false;
}

bool operator<=( const DollarAmount & left, const int & right )
{
	if ( left.amount <= right )
		return true;
	return false;
}

bool operator>( const DollarAmount& left, const int& right )
{
	if ( left.amount > right )
		return true;
	return false;
}

bool operator>=( const DollarAmount & left, const int & right )
{
	if ( left.amount >= right )
		return true;
	return false;
}

bool operator==( const DollarAmount & left, const int & right )
{
	if ( left.amount == right )
		return true;
	return false;
}

bool operator<=( const DollarAmount & left, const DollarAmount & right )
{
	if ( left.amount <= right.amount )
		return true;
	return false;
}

bool operator>( const DollarAmount & left, const DollarAmount & right )
{
	if ( left.amount > right.amount )
		return true;
	return false;
}

bool operator>=( const DollarAmount & left, const DollarAmount & right )
{
	if ( left.amount >= right.amount )
		return true;
	return false;
}

bool operator==( const DollarAmount & left, const DollarAmount & right )
{
	if ( left.amount == right.amount )
		return true;
	return false;
}

std::ostream& operator<<( std::ostream& os,const DollarAmount& amount )
{
	os << amount.toString();
	return os;
}

void DollarAmount::setAmount ( double value ) { amount = ((int)( value  * 100)); }

void DollarAmount::operator=( int right ) { amount = right; }

DollarAmount DollarAmount::operator+( DollarAmount right ) { return amount + right.amount; }

DollarAmount DollarAmount::operator-( DollarAmount right ) { return amount - right.amount; }

void DollarAmount::addInterest ( int rate, int divisor )
{
	DollarAmount interest;
	interest.setAmount (( amount * rate + divisor / 2 ) / divisor );

	amount = interest.amount + amount;
}

void DollarAmount::addInterest ( double rate )
{
	std::string rateStr = std::to_string ( rate );
	size_t indexOfDecimal = rateStr.find ( "." );
	size_t lastNonZero{ indexOfDecimal };

	for ( size_t i{ rateStr.size () - 1 }; i > indexOfDecimal; --i ) { if ( rateStr.at ( i ) != '0' ) { lastNonZero = i; break; } }

	size_t nonZerosAfterDecimal = lastNonZero - indexOfDecimal;

	std::string noDecimal_noTrailingZeros = rateStr.erase ( lastNonZero + 1 ).replace ( indexOfDecimal, 1, "" );

	double divisorDbl = pow ( 10, nonZerosAfterDecimal + 2 );
	int divisorInt = static_cast< int >( divisorDbl );
	int convertedRate = std::stoi ( noDecimal_noTrailingZeros );

	addInterest ( convertedRate, divisorInt );
}

std::string DollarAmount::toString () const
{
	std::string dollars{ std::to_string ( amount / 100 ) };
	std::string cents{ std::to_string ( std::abs ( amount % 100 ) ) };
	return dollars + "." + ( cents.size () == 1 ? "0" : "" ) + cents;
}

int64_t DollarAmount::toPennies () { return amount * 100; }
