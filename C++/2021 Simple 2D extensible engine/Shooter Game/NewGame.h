#pragma once
#include"GameObject.h"
#include"Projectile.h"
#include"Player.h"
#include"Enemy.h"
#include<random>
#include<time.h>

class NewGame: public GameObject
{
public:
	NewGame ();

	void Start () override;

	void Update ( float deltaTime ) override;

	void RegisterProjectile ( Projectile* projectile );

private:
	Player* player;
	vector<Projectile*> projectiles;
	vector<Enemy*> enemies;
	vector<Vector3> spawnPositions;
	float elapstedTime = 0;
	float spawnTime = 1;
	float windowWidth;
	float windowHeight;

	void CreateArrow ();

	void CreateArrowPoint ( Vector3 position );

	void CheckForProjectileColisions ();

	bool CheckForEnemyCollision ( vector<Projectile*>& pRemove, vector<Enemy*>& eRemove, Projectile* i, Enemy* j );

	bool CircleColision ( CircleRenderShape*& shape1, RectangleRenderShape*& shape2 );

	void SpawnEnemy ();
};