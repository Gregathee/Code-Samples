#pragma once
#include"GameObject.h"
#include"Projectile.h"

class NewGame;
class Player: public GameObject
{
public:
	Player ( RenderShape* shape, NewGame* gm ) : GameObject ( shape ) { gameManager = gm; }

	void Update ( float deltaTime );

private:
	float speed = 300;
	float rotateSpeed = 100;
	NewGame* gameManager;

	void Move ( float deltaTime );

	void Shoot ();

	Vector2 MovementDirection ();
};