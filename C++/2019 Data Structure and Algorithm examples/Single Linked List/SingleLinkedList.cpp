#ifndef _SINGLELINKEDLIST_CPP_
#define _SINGLELINKEDLIST_CPP_

#include "SingleLinkedList.h"
#include<iostream>

using std::cout;
using std::endl;
using std::pair;

SingleLinkedList::SingleLinkedList() {}
SingleLinkedList::~SingleLinkedList() {}

SLLNode* tempNodePtr1{ nullptr };
SLLNode* tempNodePtr2{ nullptr };
SLLNode* tempNodePtr3{ nullptr };
SLLNode* tempNodePtr4{ nullptr };

pair<bool, SLLNode* > SingleLinkedList::search(int target)
{
	pair<bool, SLLNode*> tempPair(false, nullptr);
	SLLNode* tempNodePtr{ headPtr };
	while (tempNodePtr != nullptr && tempNodePtr->getElement() != target) { tempNodePtr = tempNodePtr->getNextPtr(); }

	if (tempNodePtr != nullptr)
	{
		if (tempNodePtr->getElement() == target)
		{
			tempPair.first = true;
			tempPair.second = tempNodePtr;
		}
	}
	return tempPair;
}

void searchCount2TargetIsTail(SLLNode* &headPtr, SLLNode* &tailPtr)
{
	tempNodePtr1->setNextPtr(headPtr);
	tailPtr = headPtr;
	headPtr = tempNodePtr1;
	tailPtr->setNextPtr(nullptr);
}

bool SingleLinkedList::search_mtf(int target)
{
	pair<bool, SLLNode*> tempPair = search(target);
	tempNodePtr1= tempPair.second;
	bool isFound = tempPair.first;
	if (isFound)
	{
		if (count == 1 || headPtr->getElement() == target){return true;}
		else if (count == 2 && tailPtr->getElement() == target)
		{
			searchCount2TargetIsTail(headPtr, tailPtr);
			return true;
		}
		else if (count == 2){return true;}
		else
		{
			if (tailPtr->getElement() == target)
			{
				tempNodePtr2 = headPtr;
				while (tempNodePtr2->getNextPtr()->getElement() != target) { tempNodePtr2 = tempNodePtr2->getNextPtr(); }

				tempNodePtr1->setNextPtr(headPtr);
				headPtr = tempNodePtr1;
				tailPtr = tempNodePtr2;
				tailPtr->setNextPtr(nullptr);
			}
			else
			{
				tempNodePtr2 = headPtr;
				while (tempNodePtr2->getNextPtr()->getElement() != target) { tempNodePtr2 = tempNodePtr2->getNextPtr(); }

				tempNodePtr3 = tempNodePtr1->getNextPtr();
				tempNodePtr1->setNextPtr(headPtr);
				headPtr = tempNodePtr1;
				tempNodePtr2->setNextPtr(tempNodePtr3);
			}
			return true;
		}
	}
	return false;
}

bool SingleLinkedList::search_t(int target)
{
	pair<bool, SLLNode*> tempPair = search(target);
	tempNodePtr1 = tempPair.second;
	bool isFound = tempPair.first;
	if (isFound)
	{
		bool targetIsTail = (target == tailPtr->getElement());
		if (count == 1 || headPtr->getElement() == target){return true;}
		else if (count == 2 && tailPtr->getElement() == target)
		{
			searchCount2TargetIsTail(headPtr, tailPtr);
			return true;
		}
		else if (count == 2){return true;}
		else if (count == 3 && tailPtr->getElement() != target) {return search_mtf(target);}
		else if (count == 3)
		{
			tailPtr->setNextPtr(headPtr->getNextPtr());
			headPtr->setNextPtr(tailPtr);
			tailPtr->getNextPtr()->setNextPtr(nullptr);
			tailPtr = tailPtr->getNextPtr();
			return true;
		}
		else if (targetIsTail)
		{
			tempNodePtr1 = headPtr;
			while (tempNodePtr1->getNextPtr()->getNextPtr()->getElement() != target) { tempNodePtr1 = tempNodePtr1->getNextPtr(); }

			tempNodePtr2 = tempNodePtr1->getNextPtr();
			tailPtr->setNextPtr(tempNodePtr2);
			tempNodePtr1->setNextPtr(tailPtr);
			tailPtr = tempNodePtr2;
			tailPtr->setNextPtr(nullptr);
			return true;
		}
		else if (headPtr->getNextPtr()->getElement() == target) {return search_mtf(target);}
		else
		{
			tempNodePtr1 = headPtr;
			while (tempNodePtr1->getNextPtr()->getNextPtr()->getElement() != target) { tempNodePtr1 = tempNodePtr1->getNextPtr(); }

			tempNodePtr2 = tempNodePtr1->getNextPtr();
			tempNodePtr3 = tempNodePtr2->getNextPtr();
			tempNodePtr4 = tempNodePtr3->getNextPtr();

			tempNodePtr3->setNextPtr(tempNodePtr2);
			tempNodePtr1->setNextPtr(tempNodePtr3);
			tempNodePtr2->setNextPtr(tempNodePtr4);
		}
	}
	return false;
}


void SingleLinkedList::insert(int element)
{
	bool isFound = search(element).second;
	SLLNode* newNodePtr = new SLLNode(element);
	if (count != 0)
	{
		if (!isFound)
		{
			tailPtr->setNextPtr(newNodePtr);
			tailPtr = newNodePtr;
		}
		else{cout << "Element is already in list" << endl << endl;}
	}
	else
	{
		headPtr = newNodePtr;
		tailPtr = newNodePtr;
	}
	if (!isFound){++count;}
}


void SingleLinkedList::showSOLL()
{
	if (count != 0)
	{
		cout << "Displaying List elements..." << endl;
		SLLNode* tempNodePtr{ headPtr };
		for (int i = count; i > 1; --i)
		{
			std::cout << tempNodePtr->getElement() << ",\t";
			tempNodePtr = tempNodePtr->getNextPtr();
		}
		std::cout << tempNodePtr->getElement() << std::endl;
	}
	else{cout << "List is empty" << endl;}
}
#endif