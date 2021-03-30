#pragma once
#include"Color.h"
#include"Vector2.h"

//Interface for objects that are rendered on screen
class RenderShape
{
public:
	virtual RenderShape* Clone () = 0;

	virtual void SetFillColor ( Color newColor ) = 0;

	virtual void SetPosition ( Vector2& newPosition ) = 0;

	virtual Vector2 GetPosition () = 0;

protected:
	Color* color;
	Vector2 position;
};