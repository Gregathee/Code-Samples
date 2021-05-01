#include "Button.h"

Button::Button () { name = "Unamed Button"; }

Button::Button ( std::string name ) { this->name = name; }
void Button::Execute () { for ( Command* command : commands ) { command->Execute (); } }
void Button::Undo () { for ( Command* command : commands ) { command->Undo (); } }
void Button::AddCommand ( Command* command ) { commands.push_back ( command ); }
std::string Button::GetName () { return name; }
int Button::Size () { return commands.size (); }

void Button::ClearMacro ()
{
	for ( int i = commands.size () - 1; i >= 0; i-- )
	{
		delete commands[ i ];
		commands.erase ( commands.begin () + i );
	}
}

std::string Button::GetCommandNames ()
{
	std::string description = name + " Commands: ";
	for ( Command* command : commands ) { description += "\n\t" + command->Name (); }
	return description;
}

