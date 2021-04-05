#pragma once
#include"CircleRenderShape.h"
#include"SFMLRenderShape.h"
#include <SFML/Graphics.hpp>

//SFML implementation of CircleRenderShape
class SFMLCircleRenderShape: public CircleRenderShape, public SFMLRenderShape
{
public:
	SFMLCircleRenderShape ();

	SFMLCircleRenderShape (float newRadius );

	RenderShape* Clone () override;

	void SetFillColor ( Color newColor ) override;

	void SetPosition ( Vector2& newPosition ) override;

	Vector2 GetPosition ();

	float GetRadius () override;

	void SetRadius ( float newRadius ) override;

	void SetOutlineColor ( Color newColor ) override;

	void SetOutlineThickness ( float thickness ) override;

	sf::Shape* GetSFMLShape () override;

	virtual void SetRotation ( float newRotation ) override;

	virtual float GetRotation () override;

private:
	sf::CircleShape shape;
};