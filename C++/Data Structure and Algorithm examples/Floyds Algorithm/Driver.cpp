#include<iostream>
#include<string>
#include<vector>
#include<array>
#include<math.h>

using std::cout;
using std::cin;
using std::endl;
using std::array;
using std::vector;

void floyd2( const vector< vector<double>> &W, vector< vector<double>> &D, vector< vector<double>> &P);
void printMatrix( vector<vector<double>> m);
void path(vector<vector<double>> m, double q, double r);

double x = std::numeric_limits<double>::infinity();

int main()
{
	vector<double> v0, v1,  v2,  v3,  v4,  v5,  v6,  v7, v8, v9, v10, v11, v12, v13, v14;

	//                1  2  3   4  5  6  7 
	        v0 = { x, x, x, x,  x, x, x, x };
	/* 1 */ v1 = { x, 0, 4, x,  x, x,10, x };
	/* 1 */ v2 = { x, 3, 0, x, 18, x, x, x };
	/* 1 */ v3 = { x, x, 6, 0,  x, x, x, x };
	/* 1 */ v4 = { x, x, 5, 15, 0, 2,19, 5 };
	/* 1 */ v5 = { x, x, x, 12, 1, 0, x, x };
	/* 1 */ v6 = { x, x, x, x,  x, x, 0, 10 };
	/* 1 */ v7 = { x, x, x, x,  8, x, x, 0 };

	//                 1  2  3  4  5  6 
	        v8 =  { x, x, x, x, x, x, x };
	/* 1 */ v9 =  { x, 0, x, 3, x, x,-1 };
	/* 2 */ v10 = { x, x, 0, 2,12, 3, x };
	/* 3 */ v11 = { x, 3,-2, 0, x, 3, x };
	/* 4 */ v12 = { x, x,12, x, 0, x, 1 };
	/* 5 */ v13 = { x, x, 3, 3, x, 0, 1 };
	/* 6 */ v14 = { x, 1, x, x, 1, 1, 0 };

	vector<vector<double>> W, W2, D, P;
	W = { v0, v1, v2, v3, v4, v5, v6, v7 };
	D = W;
	P = W;

	floyd2(W, D, P); 

	cout << "Graph:W\n";
	printMatrix( W);

	cout << "Graph:D\n";
	printMatrix( D);

	cout << "Graph:P\n";
	printMatrix( P);

	cout << "Path from v7 to v3" << endl << "\t";
	path(P, 7, 3);
	cout << endl;

	W2 = { v8, v9, v10, v11, v12, v13, v14 };
	D = W2;
	P = W2;

	floyd2(W2, D, P);

	cout << "Graph:W2\n";
	printMatrix(W2);

	cout << "Graph:D2\n";
	printMatrix(D);

	cout << "Graph:P2\n";
	printMatrix(P);

	cout << "Path from v1 to v2" << endl << "\t";
	path(P, 1, 2);
	cout << endl;

	cout << endl << endl;
	system("pause");
}

void printMatrix( vector<vector<double>> m)
{
	int n = m[0].size();
	cout << "\t";

	for (int i = 1; i < n; i++) { cout << i << ":,\t"; }

	cout << endl;

	for (int i = 1; i < n; i++)
	{
		cout << i << ":\t";
		for (int j = 1; j < n; j++) { cout << m[i][j] << ",\t"; }
		cout << endl;
	}
}

void floyd2( const vector< vector<double>> &W, vector< vector<double>> &D, vector< vector<double>> &P)
{
	int i, j, k, n, c;
	n = W[1].size();

	for (i = 1; i < n; i++)
	{
		for (j = 1; j < n; j++) {P[i][j] = 0;}
	}

	D = W;

	for (k = 1; k < n; k++)
	{
		for (i = 1; i < n; i++)
		{
			for (j = 1; j < n; j++)
			{
				if (abs(D[i][k] + D[k][j]) < D[i][j])
				{
					P[i][j] = k;
					D[i][j] = D[i][k] + D[k][j];
				}
			}
		}
	}
}

void path(vector<vector<double>> m, double q, double r)
{
	if (m[q][r] != 0 )
	{
		path(m, q, m[q][r]);
		cout << "v" << m[q][r] << " => ";
		path(m, m[q][r], r);
	}
}