#include "LeftestHeap.h"
#include<algorithm>
#include<iostream>

using std::cout;
using std::endl;

void LeftistHeap::adjust ()
{
	if ( !left && !right ) { return; }
	else if ( !left )
	{ 
		LeftistHeap*  tempHeap = new LeftistHeap( *right );
		left = tempHeap; 
		right = nullptr; 
	}
	else if ( left->getDepth () < right->getDepth () )
	{
		LeftistHeap* tempHeap = left;
		left = right;
		right = tempHeap;
	}
}

LeftistHeap::LeftistHeap (){}

LeftistHeap::LeftistHeap ( int valueIn ):rootValue{valueIn}{}

LeftistHeap::LeftistHeap ( LeftistHeap& oldHeap )
{
	left = new LeftistHeap ();
	right = new LeftistHeap ();
	left = oldHeap.left;
	right = oldHeap.right;
	rootValue = oldHeap.rootValue;
}

LeftistHeap::~LeftistHeap () { delete( right ); delete( left ); }

void LeftistHeap::insert(int num)
{
	LeftistHeap* tempHeap = new LeftistHeap( num );
	LeftistHeap::insert ( tempHeap );
}

void LeftistHeap::insert ( LeftistHeap* heap )
{
	if ( heap->rootValue < rootValue ) { merge ( heap ); return; }
	else if ( !right ) { ;  right = heap; }
	else {  right->insert ( heap ); }
	adjust ();
}

LeftistHeap* LeftistHeap::merge( LeftistHeap* otherHeap)
{
	if ( rootValue <= otherHeap->rootValue )
	{
		if ( right ) { right->insert ( otherHeap ); }
		else right = otherHeap;
		return this;
	}
	else
	{
		LeftistHeap* originalHeap = new LeftistHeap(rootValue);
		originalHeap->right = right;
		originalHeap->left = left;
		rootValue = otherHeap->rootValue;
		right = otherHeap->right;
		left = otherHeap->left;
		insert ( originalHeap );
		otherHeap->right = nullptr;
		otherHeap->left = nullptr;
		return this;
	}
}

int LeftistHeap::deleteRoot()
{
	LeftistHeap* newRoot;
	int returnValue = rootValue;
	if ( right && left ) newRoot = new LeftistHeap ( *left->merge ( right ) );
	else if ( right ) newRoot = right;
	else if ( left ) newRoot = left;
	else { rootValue = NULL;  return returnValue; }
	rootValue = newRoot->rootValue;
	right = newRoot->right;
	left = newRoot->left;
	newRoot->left = nullptr;
	newRoot->right = nullptr;
	return returnValue;
}

void LeftistHeap::showLH()
{
	if ( right ) {  right->showLH (); }
	else cout << "x, ";
	cout << rootValue << ", ";
	if ( left ) left->showLH ();
	else cout << "x, ";
}

void LeftistHeap::showSPL()
{
	if(right ) right->showSPL ();
	cout << getDepth () << ", ";
	if ( left ) left->showSPL ();
}

int LeftistHeap::getDepth ()
{
	if ( !left  || !right ) { return 0; }
	else { return ( 1 + std::min ( left->getDepth (), right->getDepth () ) ); }
}

int LeftistHeap::getValue (){return rootValue;}
