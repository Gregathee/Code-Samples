#pragma once
#include <SFML/Graphics.hpp>

//Interface for SFML implementations of Render Shapes
class SFMLRenderShape
{
public:
	// used to get SFML shape for SFML window to draw
	virtual sf::Shape* GetSFMLShape () = 0;
};