#pragma once
#include<chrono>

class GameTime
{
public:
	float timeScale = 1;

	static GameTime* GetInstance ();

	//Used to generate delta time
	float Tick ();

private:
	GameTime ();

	static GameTime* instance;
	std::chrono::system_clock::time_point timePoint;
};