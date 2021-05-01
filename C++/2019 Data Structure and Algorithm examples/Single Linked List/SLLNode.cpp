#ifndef _SLLNODE_CPP_
#define _SLLNODE_CPP_

#include"SLLNode.h"
#include<iostream>

SLLNode::SLLNode() {}

SLLNode::~SLLNode() {delete nextNodePtr;}

SLLNode::SLLNode(const SLLNode &oldNode) {this->element = oldNode.element;}

SLLNode::SLLNode(int element) : element{ element } {}

 void SLLNode::setElement(int element){this->element = element;}

 int SLLNode::getElement() { return element; }

 void SLLNode::setNextPtr(SLLNode* nextNode) {nextNodePtr =	nextNode;}

 SLLNode* SLLNode::getNextPtr() {return nextNodePtr;}

#endif