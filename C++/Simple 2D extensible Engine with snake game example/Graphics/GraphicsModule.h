#pragma once
#include"GameWindow.h"
#include"RectangleRenderShape.h"
#include"CircleRenderShape.h"

//Interface for GraphicsModules
//The engine was originally designed using SFML but I decided to seperate SFML from the engine 
//in case I wanted to integrate another graphics frame work in it's place
class GraphicsModule
{
public:
	virtual GameWindow* CreateWindow () = 0;
	virtual CircleRenderShape* CreateCircle (float radius) = 0;
	virtual RectangleRenderShape* CreateRectangle (float width, float height) = 0;
};