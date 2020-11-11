#include<iostream>
#include<string>
#include<array>

using std::cout;
using std::cin;
using std::endl;
using std::string;
using std::array;

array<int, 15> S{ 2, 3, 5, 8, 12, 1, 7, 10, 13, 30, 6, 14, 15,18, 22 };

int quickSelect ( int low, int high, int k );
void partition ( int low, int high, int& pivotPoint );

int main ()
{
	cout << "QuickSelect(4) returns " << quickSelect ( 0, 14, 4 ) << endl;;
	cout << "QuickSelect(8) returns " << quickSelect ( 0, 14, 8 ) << endl;
	cout << "QuickSelect(12) returns " << quickSelect ( 0, 14, 12 ) << endl;

	cout << endl;
	system ( "pause" );
	return 0;
}

int quickSelect ( int low, int high, int k )
{
	int pivotpoint;

	if ( low == high ) return S[ low ];
	else
	{
		partition ( low, high, pivotpoint );
		if ( k == pivotpoint ) return S[ pivotpoint ];
		else if ( k < pivotpoint ) return quickSelect ( low, pivotpoint - 1, k );
		else return quickSelect ( pivotpoint + 1, high, k );
	}
}

void partition ( int low, int high, int& pivotPoint )
{
	int j = low;
	int pivotItem = S[low];
	for ( int i = low + 1; i <= high; i++ )
	{
		if ( S[ i ] < pivotItem )
		{
			j++;
			int temp = S[ i ];
			S[ i ] = S[ j ];
			S[ j ] = temp;
		}
	}
	pivotPoint = j;
	int temp = S[ low ];
	S[ low ] = S[ pivotPoint ];
	S[ pivotPoint ] = temp;
}