#include "Projectile.h"

Projectile::Projectile ( RenderShape* shape, Vector3 newTarget ) : GameObject ( shape )
{
	targetDir = newTarget;
}

void Projectile::Update ( float deltaTime )
{
	transform.Position += targetDir * speed * deltaTime;
	elapsedTime += deltaTime;
	if ( elapsedTime >= lifeTime ) { dead = true; }
}

int Projectile::GetDamage () { return damage; }

bool Projectile::Dead () { return dead; }
