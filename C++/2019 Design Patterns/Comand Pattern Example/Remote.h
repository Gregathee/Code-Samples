#pragma once
#include<vector>
#include"Command.h"
#include<string>
#include"Button.h"
class Remote
{
private:
	void SetMacro ( Button& button );
	void DisplayButtonCommands (Button& button);
public:
	Button button1;
	Button button2;
	Button button3;
	Button button4;
	Button button5;
	Button button6;
	std::vector<Button> undoButton;

	Remote();

	void Execute ( Button& button );
	void Undo ();

	bool Display ();
	void DisplayProgrammedCommands ();
	void SetMacroMenu ();
	void ClearMacro ();
};