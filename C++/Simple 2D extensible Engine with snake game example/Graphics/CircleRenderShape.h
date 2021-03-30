#pragma once
#include"RenderShape.h"

//Interface for primitive Circle Shapes
class CircleRenderShape: public RenderShape
{
public:
	CircleRenderShape () {}
	CircleRenderShape ( float newRadius ) { SetRadius ( newRadius ); }
	virtual RenderShape* Clone () = 0;
	virtual float GetRadius () { return radius; }
	virtual void SetRadius ( float newRadius ) = 0;
	virtual void SetOutlineColor ( Color color ) = 0;
	virtual void SetOutlineThickness ( float thickness ) = 0;
protected:
	float radius = 0;
};