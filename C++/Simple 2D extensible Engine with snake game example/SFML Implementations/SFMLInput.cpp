#include "SFMLInput.h"

void SFMLInput::UpdateKeys ()
{
	UpdateKey ( KeyCode::A, sf::Keyboard::isKeyPressed ( sf::Keyboard::A ) );
	UpdateKey ( KeyCode::D, sf::Keyboard::isKeyPressed ( sf::Keyboard::D ) );
	UpdateKey ( KeyCode::S, sf::Keyboard::isKeyPressed ( sf::Keyboard::S ) );
	UpdateKey ( KeyCode::W, sf::Keyboard::isKeyPressed ( sf::Keyboard::W ) );
	UpdateKey ( KeyCode::Escape, sf::Keyboard::isKeyPressed ( sf::Keyboard::Escape ) );
}

void SFMLInput::UpdateKey ( KeyCode code, bool update )
{
	auto key = keys.find ( code );
	if ( key == keys.end () ) return;
	key->second->Update ( update );
}
