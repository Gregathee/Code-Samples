#pragma once
#include<string>
#include"RenderShape.h"
#include"EngineEvent.h"

//Interface for GameWindow
class GameWindow
{
public:
	virtual void Create ( unsigned int width, unsigned int height, std::string name ) = 0;
	virtual void EnableVerticalSync ( bool enabled ) = 0;
	virtual float GetWidth () = 0;
	virtual float GetHeight () = 0;
	virtual void Clear () = 0;
	virtual void Draw ( RenderShape* shape ) = 0;
	virtual void Display () = 0;
	virtual bool IsOpen () = 0;
	virtual void Close () = 0;
	virtual bool PollEvent (EngineEvent& event) = 0;
};