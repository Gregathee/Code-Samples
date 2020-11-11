#include<iostream>
#include"LeftestHeap.h"

using  std::cout;
using std::endl;

int main()
{
	LeftistHeap heap1 ( 5 );
	heap1.insert ( 4 );
	heap1.insert ( 3 );
	heap1.insert ( 2 );
	heap1.insert ( 1 );

	LeftistHeap heap2 ( 5 );
	heap2.insert ( 4 );
	heap2.insert ( 3 );
	heap2.insert ( 2 );
	heap2.insert ( 1 );

	LeftistHeap heap3 ( 50 );
	heap3.insert ( 40 );
	heap3.insert ( 30 );
	heap3.insert ( 20 );
	heap3.insert ( 10 );

	LeftistHeap heap4 ( 10 );
	heap4.insert ( 20 );
	heap4.insert ( 30 );
	heap4.insert ( 40 );
	heap4.insert ( 50 );

	cout << "heap1 showLH" << endl;
	heap1.showLH ();
	cout << endl;
	cout << "heap1 showSPL" << endl;
	heap1.showSPL ();
	cout << endl;

	cout << "heap3 showLH" << endl;
	heap3.showLH ();
	cout << endl;
	cout << "heap3 showSPL" << endl;
	heap3.showSPL ();
	cout << endl;
	cout << "heap3 deleteRoot" << endl;
	heap3.deleteRoot ();
	cout << "heap3 showLH" << endl;
	heap3.showLH ();
	cout << endl;
	cout << "heap3 showSPL" << endl;
	heap3.showSPL ();
	cout << endl;

	cout << "merge heap4 into heap2" << endl;
	heap2.merge ( &heap4 );
	cout << "heap2 showLH" << endl;
	heap2.showLH ();
	cout << endl;
	cout << "heap2 showSPL" << endl;
	heap2.showSPL (); 
	cout << endl;

	cout << endl;
	system("pause");
}

