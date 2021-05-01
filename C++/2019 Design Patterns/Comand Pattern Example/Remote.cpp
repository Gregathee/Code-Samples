#include "Remote.h"
#include"RadioOnOff.h"
#include"TvOnOff.h"
#include"LightsOnOff.h"
#include"BlindsOpenClose.h"
#include"GarageDoorOpenClose.h"
#include"AlarmOnOff.h"
#include"FanSpeed.h"

using std::cout;
using std::cin;
using std::endl;

void PromptCommand ()
{
	cout << "Choose a command." << endl <<
		"1. Radio switch." << endl <<
		"2. Tv switch." << endl <<
		"3. Light switch." << endl <<
		"4. Blinds switch." << endl <<
		"5. Garage door switch." << endl <<
		"6. Alarm switch." << endl <<
		"7. Fan speed." << endl <<
		"8. Back." << endl << endl <<
		"Selection: ";
}

bool AddCommand ( bool& done, Button& button )
{
	PromptCommand ();
	int selection;
	cin >> selection;
	cout << endl;
	switch ( selection )
	{
	case 1: button.AddCommand ( new RadioOnOff () ); break;
	case 2: button.AddCommand ( new TvOnOff () ); break;
	case 3: button.AddCommand ( new LightsOnOff () ); break;
	case 4: button.AddCommand ( new BlindsOpenClose () ); break;
	case 5: button.AddCommand ( new GarageDoorOpenClose () ); break;
	case 6: button.AddCommand ( new AlarmOnOff () ); break;
	case 7: button.AddCommand ( new FanSpeed () ); break;
	case 8: done = true; break;
	default: cout << "Please enter a number 1-8." << endl; return false;
	}
	return true;
}

bool AddAnotherCommand (bool& done, Button&button)
{
	cout << endl << "Would you like to add another command to " << button.GetName() << "?" << endl <<
		"1. Yes." << endl <<
		"2. No." << endl << endl <<
		"Selection: ";
	int selection;
	cin >> selection;
	cout << endl;
	switch ( selection )
	{
	case 1: break;
	case 2: done = true; break;
	default:  cout << "Please enter a number 1-2." << endl; return false;
	}
	cout << "return true" << endl;
	return true;
}

bool PromptOverwrite (Button& button)
{
	cout << "Overwrite " << button.GetName() << "?" << endl <<
		"1. Yes. " << endl <<
		"2. No. " << endl << endl <<
		"Selection: ";
	bool validInput = false;
	int selection;
	while ( !validInput )
	{
		validInput = true;
		cin >> selection;
		cout << endl;
		switch ( selection )
		{
		case 1: break;
		case 2: return false;
		default: cout << "Please enter a number 1 - 2." << endl; validInput = false; break;
		}
	}
	return true;
}

void DisplayButtons ( std::string additionalText = "" )
{
	cout << "Choose a button" << additionalText << "." << endl <<
		"1. Button 1." << endl <<
		"2. Button 2." << endl <<
		"3. Button 3." << endl <<
		"4. Button 4." << endl <<
		"5. Button 5." << endl <<
		"6. Button 6." << endl <<
		"7. Back." << endl << endl <<
		"Selection: ";
}

void PrintDisplayOptions ()
{
	cout << "*****************" << endl <<
		"     Main Menu" << endl <<
		"*****************" << endl <<
		"Choose a button." << endl <<
		"1. Button 1." << endl <<
		"2. Button 2." << endl <<
		"3. Button 3." << endl <<
		"4. Button 4." << endl <<
		"5. Button 5." << endl <<
		"6. Button 6." << endl <<
		"7. Undo." << endl <<
		"8. Set Macro." << endl <<
		"9. See Button Commands." << endl <<
		"10. Quit. " << endl << endl <<
		"Selection: ";
}

Remote::Remote ()
{
	button1 = Button ( "Button 1" );
	button2 = Button ( "Button 2" );
	button3 = Button ( "Button 3" );
	button4 = Button ( "Button 4" );
	button5 = Button ( "Button 5" );
	button6 = Button ( "Button 6" );
}

void Remote::SetMacro ( Button& button)
{
	bool overwrite = true;
	if ( button.Size () != 0 ) { overwrite = PromptOverwrite (button); }
	if ( overwrite )
	{
		button.ClearMacro ();
		bool done = false;
		while ( !done )
		{
			while ( !AddCommand ( done, button ) ) {};
			while ( !done &&  !AddAnotherCommand ( done, button ) )  {};
		}
	}
}

void Remote::DisplayButtonCommands ( Button& button )
{
	if ( button.Size () != 0 ) { cout << button.GetCommandNames() << endl << endl; }
	else { cout << "No Commands" << endl << endl; }
}

void Remote::Execute ( Button&  button )
{
	if ( button.Size () != 0 )  { button.Execute (); } 
	else { cout << "This button does not have any commands." << endl << endl; }
	cout << endl;
	undoButton.push_back ( button );
}

void Remote::Undo ()
{
	if ( undoButton.size () != 0 ) 
	{
		for ( Button button : undoButton ) 
		{
			button.Undo (); 
			undoButton.pop_back ();
		} 
	}
	else { cout << "Nothing to undo." << endl << endl; }
	cout << endl;
}

bool Remote::Display ()
{
	PrintDisplayOptions ();
	bool validInput = 0;
	while ( !validInput )
	{
		validInput = true;
		int selection;
		cin >> selection;
		cout << endl;
		switch ( selection )
		{
		case 1: Execute ( button1 );  break;
		case 2: Execute ( button2 );  break;
		case 3: Execute ( button3 );  break;
		case 4: Execute ( button4 );   break;
		case 5: Execute ( button5 );  break;
		case 6: Execute ( button6 );  break;
		case 7: Undo(); break;
		case 8: SetMacroMenu ();  break;
		case 9: DisplayProgrammedCommands (); break;
		case 10: return false; break;
		default: cout << "Please enter 1-10." << endl; validInput = false;  break;
		}
	}
	return true;
}

void Remote::DisplayProgrammedCommands ()
{
	bool validInput = false;
	while ( !validInput )
	{
		DisplayButtons ();
		int selection;
		cin >> selection;
		cout << endl;
		validInput = true;
		switch ( selection )
		{
		case 1: DisplayButtonCommands ( button1 ); break;
		case 2: DisplayButtonCommands ( button2 ); break;
		case 3: DisplayButtonCommands ( button3 ); break;
		case 4: DisplayButtonCommands ( button4 ); break;
		case 5: DisplayButtonCommands ( button5 ); break;
		case 6: DisplayButtonCommands ( button6 ); break;
		case 7:  break;
		default: cout << "Please enter a number 1-7." << endl;  validInput = false;
		}
	}
}

void Remote::SetMacroMenu ()
{
	bool validInput = false;
	while ( !validInput )
	{
		DisplayButtons (" to assign a macro to");
		int selection;
		cin >> selection;
		cout << endl;
		validInput = true;
		switch ( selection )
		{
		case 1: SetMacro ( button1 ); break;
		case 2: SetMacro ( button2 ); break;
		case 3: SetMacro ( button3 ); break;
		case 4: SetMacro ( button4 ); break;
		case 5: SetMacro ( button5 ); break;
		case 6: SetMacro ( button6 ); break;
		case 7:  break;
		default: cout << "Please enter a number 1-7." << endl;  validInput = false;
		}
	}
}

void Remote::ClearMacro ()
{
	Button* button = nullptr;
	bool validInput = false;
	bool cancel = false;
	while ( !validInput )
	{
		DisplayButtons ();
		int selection;
		cin >> selection;
		cout << endl;
		validInput = true;
		switch ( selection )
		{
		case 1: button = &button1; break;
		case 2: button = &button2; break;
		case 3: button = &button3; break;
		case 4: button = &button4; break;
		case 5: button = &button5; break;
		case 6: button = &button6; break;
		case 7:  cancel = true;  break;
		default: cout << "Please enter a number 1-7." << endl;  validInput = false;
		}
	}
	if ( !cancel ) { button->ClearMacro (); }
}
