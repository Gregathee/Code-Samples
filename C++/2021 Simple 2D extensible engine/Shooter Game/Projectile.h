#pragma once
#include"GameObject.h"

class Projectile : public GameObject
{
public:
	Projectile ( RenderShape* shape, Vector3 newTarget );

	void Update ( float deltaTime );

	int GetDamage ();

	//After projectile has existeted for its life span, it is marked dead so the game manager will know to destroy it.
	bool Dead ();
private:
	Vector3 targetDir;
	float speed = 600;
	float lifeTime = 3;
	float elapsedTime = 0;
	int damage = 1;
	bool dead = false;
};