#pragma once
#include<cstddef>
#include<iostream>
#include<algorithm>

using std::cout;
using std::endl;
using std::max;
using std::min;

class AVLTree
{
private:
	AVLTree* left = nullptr;
	AVLTree* right = nullptr;
	int root;
	int height;

	void normalInsert ( AVLTree* treeIn )
	{
		if( treeIn->getRoot() < root)
		{
			if ( !left) left = treeIn;
			else left->normalInsert ( treeIn );
		}
		else
		{
			if ( !right ) right = treeIn;
			else right->normalInsert ( treeIn );
		}
	}

	void AVLInsert ( AVLTree* treeIn )
	{
		if ( treeIn->getRoot () < root )
		{
			if ( !left ) left = treeIn;
			else left->AVLInsert ( treeIn );
		}
		else
		{
			if ( !right ) right = treeIn;
			else right->AVLInsert ( treeIn );
		}
		if ( needsBalance () ) balance (); 
		setTreeHeight ( height );
	}

	bool hasChildren ()
	{
		if ( left ) return true;
		else if ( right ) return true;
		else return false;
	}

	int getLength ()
	{
		int lengthR, lengthL = 0;
		if ( left ) lengthL = left->getLength ();
		if ( right ) lengthR = right->getLength ();
		return max ( lengthL, lengthR ) + 1;
	}

	bool needsBalance ()
	{
		if ( !hasChildren() ) return false;
		else if ( left && right )
		{
			int lengthR = right->getLength ();
			int lengthL = left->getLength();
			if ( max ( lengthL, lengthR ) - min ( lengthL, lengthR ) == 2 ) return true;
			return false;
		}
		else if ( left ) { if ( left->hasChildren () ) return true; }
		else { if ( right->hasChildren () ) return true; }
		return false;
	}

	enum BalanceCase {LL, RR, LR, RL};

	BalanceCase getBalanceCase ()
	{
		if ( right ) if ( right->right ) return RR;
		if ( right ) if ( right->left ) return RL;
		if ( left ) if ( left->left ) return LL;
		if ( left ) if ( left->right ) return LR;
	}

	void balance ()
	{
		switch ( getBalanceCase () )
		{
			case RR:
			{
				AVLTree* tempR ( right );
				AVLTree* tempL = nullptr;
				if ( left ) tempL = left;
				left = new AVLTree ( root, height + 1 );
				left->right = right->left;
				root = right->root;
				right = right->right;
				right->setHeight ( height + 1 );
				delete tempR;
				left->left = tempL;
			}
			break;
			case LL:
			{
				AVLTree* tempL ( left );
				AVLTree* tempR = nullptr;
				if ( right ) tempR = right;
				right = new AVLTree ( root, height + 1 );
				right->left = left->right;
				root = left->root;
				left = left->left;
				left->setHeight ( height + 1 );
				delete tempL;
				right->right = tempR;
			}
			break;
			case LR:
			{
				AVLTree* temp ( left->right );
				right = new AVLTree ( root, height + 1 );
				root = left->right->root;
				left->right = nullptr;
				delete temp;
			}
				break;
			case RL:
			{
				AVLTree* temp ( right->left );
				left = new AVLTree ( root, height + 1 );
				root = right->left->root;
				right->left = nullptr;
				delete temp;
			}
				break;
		}
	}

	void setTreeHeight (int heightIn)
	{
		height = heightIn;
		if ( left ) left->setTreeHeight ( height + 1 );
		if ( right ) right->setTreeHeight ( height + 1 );
	}

public:
	AVLTree () { height = 0; }
	AVLTree ( int rootIn, int heightIn ) : root{ rootIn }, height{ heightIn } {}

	int getRoot () { return root; }

	void insert ( int element )
	{
		if ( root != NULL )
		{
			AVLTree* tree = new AVLTree ( element, 1 );
			AVLInsert ( tree );
		}
		else
		{
			root = element;
			height = 0;
		}
		setTreeHeight (height);
	}

	bool search ( int target )
	{
		if ( target == root ) return true;
		else if ( target < root )
		{
			if ( left ) return left->search ( target );
			else return false;
		}
		else if ( target > root )
		{
			if ( right ) return right->search ( target );
			else return false;
		}
	}

	void showAv1st ()
	{
		if ( root == NULL ) return;
		if ( right ) right->showAv1st ();
		cout << root << endl;
		if ( left ) left->showAv1st ();
	}

	void showHeight ()
	{
		if ( root == NULL ) return;
		if ( right ) right->showHeight ();
		cout << height << endl;
		if ( left ) left->showHeight ();
	}

	void setHeight ( int heightIn ) { height = heightIn; }
};

