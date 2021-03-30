#include "SFMLCircleRenderShape.h"

SFMLCircleRenderShape::SFMLCircleRenderShape () { shape.setRadius ( 0 ); }

SFMLCircleRenderShape::SFMLCircleRenderShape ( float newRadius ) { SetRadius ( newRadius ); }

RenderShape* SFMLCircleRenderShape::Clone ()
{
	return new SFMLCircleRenderShape ( *this );
}

void SFMLCircleRenderShape::SetFillColor ( Color newColor )
{
	sf::Uint8 r = ( sf::Uint8 ) newColor.GetR ();
	sf::Uint8 g = ( sf::Uint8 ) newColor.GetG ();
	sf::Uint8 b = ( sf::Uint8 ) newColor.GetB ();
	sf::Uint8 a = ( sf::Uint8 ) newColor.GetA ();
	shape.setFillColor ( sf::Color ( r, g, b, a ) );
}

void SFMLCircleRenderShape::SetPosition ( Vector2& newPosition )
{
	shape.setPosition ( sf::Vector2f ( newPosition.X, newPosition.Y ) );
}

Vector2 SFMLCircleRenderShape::GetPosition () { return Vector2 ( shape.getPosition ().x, shape.getPosition ().y ); }

float SFMLCircleRenderShape::GetRadius () { return shape.getRadius (); }

void SFMLCircleRenderShape::SetRadius ( float newRadius ) { shape.setRadius ( newRadius ); }

void SFMLCircleRenderShape::SetOutlineColor ( Color newColor )
{
	sf::Uint8 r = ( sf::Uint8 ) ~newColor.GetR ();
	sf::Uint8 g = ( sf::Uint8 ) ~newColor.GetG ();
	sf::Uint8 b = ( sf::Uint8 ) ~newColor.GetB ();
	sf::Uint8 a = ( sf::Uint8 ) ~newColor.GetA ();
	shape.setOutlineColor ( sf::Color ( r, g, b, a ) );
}

void SFMLCircleRenderShape::SetOutlineThickness ( float thickness ) { shape.setOutlineThickness ( thickness ); }

sf::Shape* SFMLCircleRenderShape::GetSFMLShape () { return &shape; }
