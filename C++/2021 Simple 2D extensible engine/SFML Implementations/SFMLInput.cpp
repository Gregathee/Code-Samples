#include "SFMLInput.h"

void SFMLInput::UpdateKeys ()
{
	UpdateKey ( KeyCode::A, sf::Keyboard::isKeyPressed ( sf::Keyboard::A ) );
	UpdateKey ( KeyCode::D, sf::Keyboard::isKeyPressed ( sf::Keyboard::D ) );
	UpdateKey ( KeyCode::E, sf::Keyboard::isKeyPressed ( sf::Keyboard::E ) );
	UpdateKey ( KeyCode::S, sf::Keyboard::isKeyPressed ( sf::Keyboard::S ) );
	UpdateKey ( KeyCode::W, sf::Keyboard::isKeyPressed ( sf::Keyboard::W ) );
	UpdateKey ( KeyCode::Q, sf::Keyboard::isKeyPressed ( sf::Keyboard::Q ) );
	UpdateKey ( KeyCode::Escape, sf::Keyboard::isKeyPressed ( sf::Keyboard::Escape ) );
	UpdateKey ( KeyCode::LeftClick, sf::Mouse::isButtonPressed ( sf::Mouse::Button::Left ) );
}

Vector2 SFMLInput::GetMousePositionScreenToWorld ()
{
	SFMLGameWindow* window = static_cast< SFMLGameWindow* >( GameEngine::Instance ()->GetGameWindow () );
	sf::Vector2i v2 = sf::Mouse::getPosition (window->GetSFMLRenderWindow());
	IGameObject* target = GameEngine::Instance ()->GetCameraFollowTarget ();
	Vector2 position ( v2.x, v2.y ); 
	Vector2 screenCenter ( window->GetWidth () / 2, window->GetHeight () / 2 );
	if ( target )
	{
		if ( target ) { position = GameEngine::Instance ()->TranslateScreenPositionRelativeToCameraRotation ( position, true ); }
		Vector3 targetPos ( target->GetTransform ().Position );
		targetPos.x *= -1;
		position -= targetPos;
	}
	position.x -= window->GetWidth () / 2;
	position.y *= -1;
	position.y += window->GetHeight () / 2;
	return position;
}

void SFMLInput::UpdateKey ( KeyCode code, bool update )
{
	auto key = keys.find ( code );
	if ( key == keys.end () ) return;
	key->second->Update ( update );
}
