#include "GameTime.h"

GameTime* GameTime::instance;

GameTime::GameTime ()
{
	timePoint = std::chrono::system_clock::now ();
}

float GameTime::Tick ()
{
	std::chrono::system_clock::time_point now = std::chrono::system_clock::now ();
	std::chrono::duration<float> seconds = now - timePoint;
	timePoint = now;
	return seconds.count () * timeScale;
}

GameTime* GameTime::GetInstance ()
{
	if ( !instance ) { instance = new GameTime; }
	return instance;
}
