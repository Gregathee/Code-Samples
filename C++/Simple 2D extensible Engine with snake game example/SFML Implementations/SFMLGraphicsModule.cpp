#include "SFMLGraphicsModule.h"

GameWindow* SFMLGraphicsModule::CreateWindow ()
{
	return new SFMLGameWindow ();
}

CircleRenderShape* SFMLGraphicsModule::CreateCircle ( float radius )
{
	return new SFMLCircleRenderShape ( radius );
}

RectangleRenderShape* SFMLGraphicsModule::CreateRectangle ( float width, float height )
{
	return new SFMLRectangleRenderShape ( width, height );
}
