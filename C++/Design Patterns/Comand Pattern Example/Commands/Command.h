#pragma once
#include<iostream>
#include<string>
class Command
{
public:
	virtual void Execute () = 0;
	virtual void Undo () = 0;
	virtual std::string Name () = 0;
};
