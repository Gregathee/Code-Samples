#pragma once
#include <string>
#include <vector>
#include"Command.h"

class Button
{
private:
	std::string name;
	std::vector<Command*> commands;
public:
	Button ();
	Button ( std::string name );
	void Execute ();
	void Undo ();
	void AddCommand ( Command* command );
	void ClearMacro ();
	std::string GetCommandNames ();
	std::string GetName ();
	int Size ();
};