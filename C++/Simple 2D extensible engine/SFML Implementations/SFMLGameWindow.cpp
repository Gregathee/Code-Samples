#include "SFMLGameWindow.h"
#include"IGameObject.h"

void SFMLGameWindow::Create ( unsigned int width, unsigned int height, std::string name )
{
	window.create ( sf::VideoMode ( width, height ), name );
}

void SFMLGameWindow::EnableVerticalSync ( bool enabled )
{
	window.setVerticalSyncEnabled ( enabled );
}

void SFMLGameWindow::Clear () { window.clear (); }

void SFMLGameWindow::Draw ( RenderShape* shape )
{
	sf::Shape* renderShape = dynamic_cast< SFMLRenderShape* >( shape )->GetSFMLShape ();
	window.draw ( *renderShape );
}

void SFMLGameWindow::Display () { window.display (); }

bool SFMLGameWindow::IsOpen () { return window.isOpen (); }

void SFMLGameWindow::Close () { window.close (); }

bool SFMLGameWindow::PollEvent ( EngineEvent& event )
{
	sf::Event sfEvent;
	bool result = window.pollEvent ( sfEvent );
	switch ( sfEvent.type )
	{
	case sf::Event::Closed: event.Type = EngineEventType::Closed; break;
	default: event.Type = EngineEventType::None;
	}
	return result;
}

float SFMLGameWindow::GetWidth () { return ( float ) window.getSize ().x; }

float SFMLGameWindow::GetHeight () { return( float ) window.getSize ().y; }

sf::RenderWindow& SFMLGameWindow::GetSFMLRenderWindow ()
{
	return window;
}
