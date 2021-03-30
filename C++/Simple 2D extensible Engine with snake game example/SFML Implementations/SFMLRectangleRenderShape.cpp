#include "SFMLRectangleRenderShape.h"

SFMLRectangleRenderShape::SFMLRectangleRenderShape ( float newWidth, float newHeight ) { shape = sf::RectangleShape ( sf::Vector2f ( newWidth, newHeight ) ); }

RenderShape* SFMLRectangleRenderShape::Clone ()
{
	return new SFMLRectangleRenderShape ( *this );
}



void SFMLRectangleRenderShape::SetFillColor ( Color newColor )
{
	sf::Uint8 r = ( sf::Uint8 ) newColor.GetR ();
	sf::Uint8 g = ( sf::Uint8 ) newColor.GetG ();
	sf::Uint8 b = ( sf::Uint8 ) newColor.GetB ();
	sf::Uint8 a = ( sf::Uint8 ) newColor.GetA ();
	shape.setFillColor ( sf::Color ( r, g, b, a ) );
}

void SFMLRectangleRenderShape::SetPosition ( Vector2& newPosition )
{
	shape.setPosition ( sf::Vector2f ( newPosition.X, newPosition.Y ) );
}

Vector2 SFMLRectangleRenderShape::GetPosition () { return Vector2 ( shape.getPosition ().x, shape.getPosition ().y ); }

void SFMLRectangleRenderShape::SetOutlineColor ( Color newColor )
{
	sf::Uint8 r = ( sf::Uint8 ) newColor.GetR ();
	sf::Uint8 g = ( sf::Uint8 ) newColor.GetG ();
	sf::Uint8 b = ( sf::Uint8 ) newColor.GetB ();
	sf::Uint8 a = ( sf::Uint8 ) newColor.GetA ();
	shape.setOutlineColor ( sf::Color ( r, g, b, a ) );
}

void SFMLRectangleRenderShape::SetOutlineThickness ( float thickness )
{
	shape.setOutlineThickness ( thickness );
}

float SFMLRectangleRenderShape::GetWidth () { return shape.getSize ().x; }

float SFMLRectangleRenderShape::GetHeight () { return shape.getSize ().y; }

sf::Shape* SFMLRectangleRenderShape::GetSFMLShape () { return &shape; }
