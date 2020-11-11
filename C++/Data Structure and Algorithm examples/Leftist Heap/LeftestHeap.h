#pragma once

class LeftistHeap
{
private:
	LeftistHeap* left = nullptr;
	LeftistHeap* right = nullptr;
	int rootValue;
	void adjust ();
public:
	LeftistHeap ();
	LeftistHeap ( int valueIn );
	LeftistHeap ( LeftistHeap& oldHeap );
	~LeftistHeap ();
	void insert(int num);
	void insert ( LeftistHeap* heap );
	LeftistHeap* merge( LeftistHeap* otherHeap);
	int deleteRoot();
	void showLH();
	void showSPL();
	int getDepth ();
	int getValue ();
	LeftistHeap* getRight () { return right; }
	LeftistHeap* getLeft () { return left; }
};
