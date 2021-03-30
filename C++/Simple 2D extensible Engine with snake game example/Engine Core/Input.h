#pragma once
#include"Key.h"
#include<map>

//Interface used to maintain the status of keys per frame
class Input
{
public:
	Input ();

	virtual void UpdateKeys () = 0;

	//Is key currently being held down?
	bool GetKey ( KeyCode code );

	//Was key pressed down this frame?
	bool GetKeyDown ( KeyCode code );
	
	//Was key released this frame?
	bool GetKeyUp ( KeyCode code );

protected:
	std::map<KeyCode, Key*> keys;
};