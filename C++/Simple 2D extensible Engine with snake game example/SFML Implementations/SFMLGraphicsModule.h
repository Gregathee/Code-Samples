#pragma once
#include"SFMLGameWindow.h"
#include"SFMLCircleRenderShape.h"
#include"SFMLRectangleRenderShape.h"
#include"GraphicsModule.h"

//SFML implementation of graphics module
class SFMLGraphicsModule : public GraphicsModule
{
public:
	GameWindow* CreateWindow () override;

	CircleRenderShape* CreateCircle ( float radius );

	RectangleRenderShape* CreateRectangle ( float width, float height );
};