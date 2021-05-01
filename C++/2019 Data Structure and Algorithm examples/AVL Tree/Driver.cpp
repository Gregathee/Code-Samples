#include<iostream>
#include<string>
#include"AVLTree.h"
#include<vector>

using std::cout;
using std::cin;
using std::endl;
using std::string;
using std::vector;

int main ()
{
	vector<int> test1{ 10, 20, 30, 40, 50, 60, 70 };
	vector<int> test2{ 70, 60, 50, 40, 30, 20, 10 };

	AVLTree t1;
	AVLTree t2;

	for ( int i = 0; i < test1.size (); i++ ) { t1.insert ( test1[ i ] ); }

	for ( int i = 0; i < test2.size (); i++ ) { t2.insert ( test2[ i ] ); }

	cout << "Displaying test1 AVL tree \n\n";
	t1.showAv1st ();
	cout << "Dispalying test1 height \n\n";
	t1.showHeight ();
	cout << "Displaying test2 AVL tree \n\n";
	t2.showAv1st ();
	cout << "Dispalying test2 height \n\n";
	t2.showHeight ();

	cout << "test1.search(20) = " << t1.search ( 20 ) << endl;
	cout << "test1.search(21) = " << t1.search ( 21 ) << endl;
	cout << "test1.search(20) = " << t1.search ( 20 ) << endl;
	cout << "test1.search(21) = " << t1.search ( 21 ) << endl;

	system ( "pause" );
	return 0;
}