#include "Input.h"

Input::Input ()
{
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::A, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::D, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::E, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::S, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::Q, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::W, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::Escape, new Key () ) );
	keys.insert ( std::pair<KeyCode, Key*> ( KeyCode::LeftClick, new Key () ) );
}

bool Input::GetKey ( KeyCode code )
{
	auto key = keys.find ( code );
	if ( key == keys.end () ) return false;
	return key->second->IsPressed ();
}

bool Input::GetKeyDown ( KeyCode code )
{
	auto key = keys.find ( code );
	if ( key == keys.end () ) return false;
	return key->second->IsDownThisFrame ();
}

bool Input::GetKeyUp ( KeyCode code )
{
	auto key = keys.find ( code );
	if ( key == keys.end () ) return false;
	return key->second->IsUpThisFrame ();
}
