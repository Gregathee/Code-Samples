#pragma once
#include"GameWindow.h"
#include"SFMLRenderShape.h"
#include <SFML/Graphics.hpp>

//SFML implementation of GameWindow
class SFMLGameWindow: public GameWindow
{
public:
	void Create ( unsigned int width, unsigned int height, std::string name ) override;

	void EnableVerticalSync ( bool enabled ) override;

	void Clear () override;

	void Draw ( RenderShape* shape ) override;

	void Display () override;

	bool IsOpen () override;

	void Close () override;

	bool PollEvent ( EngineEvent& event ) override;

	float GetWidth () override;
	float GetHeight () override;
private:
	sf::RenderWindow window;
};