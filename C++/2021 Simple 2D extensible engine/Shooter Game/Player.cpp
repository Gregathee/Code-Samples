#include "Player.h"
#include "NewGame.h"

void Player::Update ( float deltaTime )
{
	Move ( deltaTime );
	Shoot ();
}

void Player::Move ( float deltaTime )
{
	Vector2 moveDir = MovementDirection ();
	Vector2 up = transform.Up2D () * moveDir.y;
	Vector2 right = transform.Right2D () * moveDir.x;
	moveDir = right + up;
	transform.Position += Vector3 ( moveDir.x, moveDir.y, 0 ) * speed * deltaTime;
	if ( engine->GetInput ()->GetKey ( KeyCode::E ) ) { transform.Rotate ( -rotateSpeed * deltaTime ); }
	if ( engine->GetInput ()->GetKey ( KeyCode::Q ) ) { transform.Rotate ( rotateSpeed * deltaTime ); }
}

void Player::Shoot ()
{
	if ( engine->GetInput ()->GetKeyDown ( KeyCode::LeftClick ) )
	{
		Vector2 v2 = engine->GetInput ()->GetMousePositionScreenToWorld ();
		Vector3 v3 ( v2.x, v2.y, transform.Position.z + -0.1f );
		v3 -= transform.Position;

		Projectile* projectile = new Projectile ( engine->GetGraphicsModule ()->CreateCircle ( 30 ), v3.Normalize () );
		projectile->GetRenderShape ()->SetFillColor ( Color ( 0, 255, 0 ) );
		projectile->GetTransform ().Position = transform.Position;
		gameManager->RegisterProjectile ( projectile );
	}
}

Vector2 Player::MovementDirection ()
{
	float horizontalMovement = 0;
	float verticalMovement = 0;
	if ( engine->GetInput ()->GetKey ( KeyCode::W ) ) { verticalMovement += 1; }
	if ( engine->GetInput ()->GetKey ( KeyCode::S ) ) { verticalMovement -= 1; }
	if ( engine->GetInput ()->GetKey ( KeyCode::D ) ) { horizontalMovement += 1; }
	if ( engine->GetInput ()->GetKey ( KeyCode::A ) ) { horizontalMovement -= 1; }
	return Vector2 ( horizontalMovement, verticalMovement ).Normalize ();
}
