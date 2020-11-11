#pragma once
#ifndef _SLLNODE_H_
#define _SLLNODE_H_

#include<stddef.h>



class SLLNode 
{
private:

	int element = NULL;
	SLLNode* nextNodePtr = nullptr;

public:
	SLLNode();
	~SLLNode();
	SLLNode(const SLLNode&);
	SLLNode(int element);

	void setElement(int element);
	int getElement(); 

	void setNextPtr(SLLNode* next);
	SLLNode* getNextPtr();
};

#endif 