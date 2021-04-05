#include "Key.h"

bool Key::IsDownThisFrame () { return isDownThisFrame; }

bool Key::IsUpThisFrame () { return isUpThisFrame; }

bool Key::IsPressed () { return isPressed; }

void Key::Update ( bool keyPressed )
{
	isDownThisFrame = keyPressed && !isPressed;
	isUpThisFrame = !keyPressed && isPressed;
	isPressed = keyPressed;
}
