#pragma once
#include"RenderShape.h"

//Interface for primitive rectangle shape
class RectangleRenderShape: public RenderShape
{
public:
	virtual RenderShape* Clone () = 0;
	virtual float GetWidth () = 0;
	virtual float GetHeight () = 0;
	virtual void SetOutlineColor ( Color color ) = 0;
	virtual void SetOutlineThickness ( float thickness ) = 0;
	virtual void SetRotation ( float newRotation ) = 0;
	virtual float GetRotation () = 0;
protected:
};