#include <iostream>
#include<vector>
#include <utility>
#include <string>

using namespace std;

double x = numeric_limits<double>::infinity ();

//construct a matrix representing the sortest distance between points
vector<pair<int, int>> dijkstra( const vector<vector<double>> W)
{
	int n = W.size ();
	int i;
	int vnear;
	pair <int, int> e;
	vector<int> touch;
	vector<double> length;

	vector<pair<int, int>> F;
	for (i = 1; i <= n-1; i++)
	{
		touch.push_back(0);
		length.push_back(W[0][i]);
	}
	for (int j = n - 1; j > 0; j--)
	{
		double min = x;
		for ( i = 1; i <= n - 1; i++ )
		{
			if ( 0 <= length[ i - 1 ] && length[ i - 1 ] < min )
			{
				min = length[ i - 1 ];
				vnear = i;
			}
		}
		e.first = touch[vnear-1];
		e.second = vnear;
		F.push_back(e);
		for ( i = 1; i <= n - 1; i++ )
		{
			if (length[ vnear - 1 ] + W[ vnear ][ i ] < length[i-1] )
			{
				length[ i - 1 ] = length[ vnear - 1 ] + W[ vnear ][ i ];
				touch[ i - 1 ] = vnear;
			}
		}
		length[vnear-1] = -1;
	}
	return F;
}

//Converts number of a vertext into a letter
string vertexNumbertoLetter ( int num)
{
	string vertex;
	switch ( num )
	{
	case 0: vertex = "A"; break;
	case 1: vertex = "B"; break;
	case 2: vertex = "C"; break;
	}
	return vertex;
}

int main()
{
	//build test graph matrixes
	vector<double> v1, v2, v3, v4, v0;
	vector<double> vA, vB, vC;

	//   v  0  1   2  3  4
	v0 = { 0, 7, 4, 6, 1 };
	v1 = { x, 0, x, x, x };
	v2 = { x, 2, 0, 5, x };
	v3 = { x, 3, x, 0, x };
	v4 = { x, x, x, 1, 0 };

	//       A B   C
	vA = { 0, 7,  5 };
	vB = { x, 0, -5 };
	vC = { x, x,  0 };

	vector<vector<double>> testCase1 = { v0, v1, v2, v3, v4 };
	vector<pair<int, int>> testCase1Results;
	vector<vector<double>> testCase2 = { vA, vB, vC };
	vector<pair<int, int>> testCase2Results;

	testCase1Results = dijkstra ( testCase1 );
	testCase2Results = dijkstra ( testCase2 );

	cout << "Test 1 Results:" << endl;
	for ( unsigned i = 0; i < testCase1Results.size () - 1; i++ ) { cout << "{ " << testCase1Results[ i ].first << ", " << testCase1Results[ i ].second << " }, "; }
	cout << "{ " << testCase1Results[ testCase1Results.size () -1].first << ", " << testCase1Results[ testCase1Results.size () -1].second << " }" << endl;

	string vertex1;
	string vertex2;

	cout << "Test 2 Results:" << endl;
	for ( unsigned i = 0; i < testCase2Results.size ( ) - 1; i++ )
	{
		vertex1 = vertexNumbertoLetter ( testCase2Results[ i ].first );
		vertex2 = vertexNumbertoLetter ( testCase2Results[ i ].second );
		cout << "{ " << vertex1 << ", " << vertex2 << " }, ";
	}
	vertex1 = vertexNumbertoLetter ( testCase2Results[ testCase2Results.size () - 1].first );
	vertex2 = vertexNumbertoLetter ( testCase2Results[ testCase2Results.size () - 1].second );
	cout << "{ " << vertex1 << ", " << vertex2 << " }" << endl;

	cout << endl;
	system("pause");
}