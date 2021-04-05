#pragma once
#include"Input.h"
#include <SFML/Graphics.hpp>
#include"GameEngine.h"

//SFML implementation of Input
class SFMLInput: public Input
{
public:
	void UpdateKeys () override;

	Vector2 GetMousePositionScreenToWorld ();

private:
	void UpdateKey ( KeyCode code, bool update );
};