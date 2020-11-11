#pragma once
#include "SLLNode.h"
#include<utility>

#ifndef _SINGLELINKEDLIST_H_
#define _SINGLELINKEDLIST_H_

class SingleLinkedList
{
private:
	SLLNode* headPtr = nullptr;
	SLLNode* tailPtr = nullptr;
	int count = 0;
	
public:
	SingleLinkedList();
	~SingleLinkedList();

	std::pair<bool, SLLNode* > search(int target);

	bool search_mtf(int target);
	
	bool search_t(int target);

	void insert(int element);

	void showSOLL();

	int size();

};

#endif 