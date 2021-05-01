#pragma once
#include"GameObject.h"
#include"Player.h"

class Enemy: public GameObject
{
public:
	Enemy ( Player* newPlayer, RenderShape* shape );

	void Update ( float deltaTime );

	void TakeDamage ( int damage );

	int GetHealth ();

private:
	Player* player;
	int health = 1;
	float speed = 100;
};