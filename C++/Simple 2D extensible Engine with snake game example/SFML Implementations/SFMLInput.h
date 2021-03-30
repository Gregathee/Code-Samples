#pragma once
#include"Input.h"
#include <SFML/Graphics.hpp>

//SFML implementation of Input
class SFMLInput: public Input
{
public:
	void UpdateKeys () override;

private:
	void UpdateKey ( KeyCode code, bool update );
};