#include "Enemy.h"

Enemy::Enemy ( Player* newPlayer, RenderShape* shape ) :GameObject ( shape ) { player = newPlayer; }

void Enemy::Update ( float deltaTime )
{
	Vector3 diff = ( player->GetTransform ().Position - transform.Position ).Normalize ();
	transform.Position += diff * deltaTime * speed;
}

void Enemy::TakeDamage ( int damage ) { health -= damage; }

int Enemy::GetHealth () { return health; }
