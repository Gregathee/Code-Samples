#include "NewGame.h"

NewGame::NewGame () : GameObject ( false ) { engine->RegisterGameObject ( this ); }

void NewGame::Start ()
{
	windowWidth = engine->GetGameWindow ()->GetWidth ();
	windowHeight = engine->GetGameWindow ()->GetHeight ();
	srand ( ( unsigned ) time ( NULL ) );
	player = new Player ( engine->GetGraphicsModule ()->CreateRectangle ( 100, 100 ), this );
	player->GetRenderShape ()->SetFillColor ( Color ( 0, 0, 255 ) );
	engine->SetCameraFollowTarget ( player );

	spawnPositions.push_back ( Vector3 ( 0, windowHeight, 0 ) );
	spawnPositions.push_back ( Vector3 ( windowWidth, windowHeight, 0 ) );
	spawnPositions.push_back ( Vector3 ( windowWidth, 0, 0 ) );
	spawnPositions.push_back ( Vector3 ( windowWidth, -windowHeight, 0 ) );
	spawnPositions.push_back ( Vector3 ( 0, -windowHeight, 0 ) );
	spawnPositions.push_back ( Vector3 ( -windowWidth, -windowHeight, 0 ) );
	spawnPositions.push_back ( Vector3 ( -windowWidth, 0, 0 ) );
	spawnPositions.push_back ( Vector3 ( -windowWidth, windowHeight, 0 ) );

	CreateArrow ();
}

void NewGame::Update ( float deltaTime )
{
	elapstedTime += deltaTime;
	if ( elapstedTime >= spawnTime ) { SpawnEnemy (); elapstedTime = 0; }
	CheckForProjectileColisions ();
}

void NewGame::RegisterProjectile ( Projectile* projectile ) { projectiles.push_back ( projectile ); }

void NewGame::CreateArrow()
{
	GameObject* org = new GameObject ( engine->GetGraphicsModule ()->CreateCircle ( 10 ) );
	org->GetRenderShape ()->SetFillColor ( Color ( 128, 0, 128 ) );

	CreateArrowPoint ( Vector3 ( 200, 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 20 + 200, -10 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( -20 + 200, -10 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 40 + 200, -20 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( -40 + 200, -20 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 0 + 200, -20 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 0 + 200, -40 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 0 + 200, -60 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 0 + 200, -80 + 200, 0 ) );
	CreateArrowPoint ( Vector3 ( 0 + 200, -100 + 200, 0 ) );
}

void NewGame::CreateArrowPoint(Vector3 position)
{
	GameObject* ob = new GameObject ( engine->GetGraphicsModule ()->CreateCircle ( 10 ) );
	ob->GetRenderShape ()->SetFillColor ( Color ( 255, 0, 0 ) );
	ob->GetTransform ().Position = position;
}

void NewGame::CheckForProjectileColisions()
{
	vector<Projectile*> pRemove;
	vector<Enemy*> eRemove;
	for ( auto i : projectiles )
	{
		if ( i->Dead () )
		{
			pRemove.push_back ( i );
			engine->DestroyGameObject ( i->ID );
			continue;
		}
		for ( auto j : enemies ) { if ( CheckForEnemyCollision ( pRemove, eRemove, i, j ) ) { break; } }
	}
	for ( auto i : pRemove ) { projectiles.erase ( std::find ( projectiles.begin (), projectiles.end (), i ) ); }
	for ( auto i : eRemove ) { enemies.erase ( std::find ( enemies.begin (), enemies.end (), i ) ); }
}

bool NewGame::CheckForEnemyCollision( vector<Projectile*>& pRemove, vector<Enemy*>& eRemove, Projectile* i, Enemy* j )
{
	CircleRenderShape* c1 = static_cast< CircleRenderShape* >( i->GetRenderShape () );
	RectangleRenderShape* c2 = static_cast< RectangleRenderShape* >( j->GetRenderShape () );
	if ( CircleColision ( c1, c2 ) )
	{
		int damage = i->GetDamage ();
		j->TakeDamage ( damage );
		if ( j->GetHealth () < 1 )
		{
			eRemove.push_back ( j );
			engine->DestroyGameObject ( j->ID );
		}
		pRemove.push_back ( i );
		engine->DestroyGameObject ( i->ID );
		return true;
	}
	return false;
}

bool NewGame::CircleColision ( CircleRenderShape*& shape1, RectangleRenderShape*& shape2 )
{
	//define minimum distance that two circles could be apart without touching
	float combinedRadius = shape1->GetRadius () + shape2->GetWidth () / 2;

	//use differences in x's and y's and use pythagorim theorm to get distances between the two circles centers
	float xDif = shape1->GetPosition ().x - shape2->GetPosition ().x;
	float yDif = shape1->GetPosition ().y - shape2->GetPosition ().y;
	float distance = sqrt ( pow ( xDif, 2.0f ) + pow ( yDif, 2.0f ) );

	//return if overlapping
	return distance < combinedRadius;
}

void NewGame::SpawnEnemy ()
{
	Enemy* enemy = new Enemy ( player, engine->GetGraphicsModule ()->CreateRectangle ( 50, 50 ) );
	enemy->GetRenderShape ()->SetFillColor ( Color ( 255, 0, 0 ) );
	int randIndex = rand () % spawnPositions.size ();
	enemy->GetTransform ().Position = spawnPositions[ randIndex ];
	enemies.push_back ( enemy );
}

