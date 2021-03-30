#pragma once
//Only contains keys that I have used so far
enum class KeyCode { A, D, S, W, Escape };

//Used to store the status of a key press per frame
class Key
{
public:
	bool IsDownThisFrame ();
	bool IsUpThisFrame ();
	bool IsPressed ();

	void Update ( bool keyPressed );

private:
	bool isDownThisFrame = false;
	bool isUpThisFrame = false;
	bool isPressed = false;
};