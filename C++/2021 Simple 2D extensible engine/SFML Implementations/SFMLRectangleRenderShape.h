#pragma once
#include"RectangleRenderShape.h"
#include"SFMLRenderShape.h"
#include <SFML/Graphics.hpp>

// SFML Implementation of Rectangle Render Shapes
class SFMLRectangleRenderShape: public RectangleRenderShape, public SFMLRenderShape
{
public:
	SFMLRectangleRenderShape () {}

	SFMLRectangleRenderShape ( float newWidth, float newHeight );

	RenderShape* Clone () override;

	void SetFillColor ( Color newColor ) override;

	void SetPosition ( Vector2& newPosition ) override;

	Vector2 GetPosition ();

	void SetOutlineColor ( Color newColor ) override;

	void SetOutlineThickness ( float thickness ) override;

	float GetWidth ();
	float GetHeight ();

	virtual void SetRotation ( float newRotation ) override;

	virtual float GetRotation () override;

	sf::Shape* GetSFMLShape () override;
private:
	sf::RectangleShape shape;
};