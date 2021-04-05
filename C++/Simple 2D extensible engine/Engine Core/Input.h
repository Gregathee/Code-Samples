#pragma once
#include"Key.h"
#include<map>
#include"Vector2.h"

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

	virtual Vector2 GetMousePositionScreenToWorld () = 0;

protected:
	std::map<KeyCode, Key*> keys;
};