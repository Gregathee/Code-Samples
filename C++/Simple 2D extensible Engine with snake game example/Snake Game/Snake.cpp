#include "Snake.h"

Snake::Snake () : GameObject ( false ) { engine->RegisterGameObject ( this ); }

Snake::Snake ( RenderShape* newRenderShape, float radius ) : GameObject ( false )
{
    engine->RegisterGameObject ( this );
    SetRenderShape ( newRenderShape );
    snakeRadius = radius;
}

void Snake::Update ( float deltaTime )
{
    ProcessInput ();
    Move ( deltaTime );
}

void Snake::AddBody ()
{
    //create new body part
    IGameObject* newBody = new GameObject ( engine->GetGraphicsModule ()->CreateCircle ( snakeRadius ) );
    Color bodyColor ( 0, 128, 0 );
    newBody->GetRenderShape ()->SetFillColor ( bodyColor );

    //if snake has a tail, place new body part at position of tail, else place at head.
    if ( snakeBody.size () > 0 ) { newBody->SetPosition ( snakeBody[ snakeBody.size () - 1 ]->GetPosition ().X, snakeBody[ snakeBody.size () - 1 ]->GetPosition ().Y ); }
    else { newBody->SetPosition ( GetPosition ().X, GetPosition ().Y ); }

    //add new body part to vectors to be drawn and repositioned each frame
    snakeBody.push_back ( newBody );
}

bool Snake::CollidedWithBody ()
{
    for ( int i = distanceForBodyColision + 5; i < snakeBody.size (); i++ )
    {
        //use differences in x's and y's and use pythagorim theorm to get distances between the two circles centers
        float xDif = GetPosition ().X - snakeBody[ i ]->GetPosition ().X;
        float yDif = GetPosition ().Y - snakeBody[ i ]->GetPosition ().Y;
        float distance = sqrt ( pow ( xDif, 2.0f ) + pow ( yDif, 2.0f ) );

        //because of the implementation of the snake body, body parts and head naturally overlap so the threshold is used to determine collision
        //return true if distances is within set threshold to determine "collision"
        if ( distance < distanceForBodyColision ) { return true; }
    }
    return false;
}

void Snake::ClearBody ()
{
    for ( auto i : snakeBody ) { engine->DestroyGameObject ( i->ID ); }
    snakeBody.clear ();
}

void Snake::ProcessInput ()
{
    if ( input->GetKey ( KeyCode::W ) && direction != Direction::Down ) { direction = Direction::Up; }
    else if ( input->GetKey ( KeyCode::D ) && direction != Direction::Left ) { direction = Direction::Right; }
    else if ( input->GetKey ( KeyCode::S ) && direction != Direction::Up ) { direction = Direction::Down; }
    else if ( input->GetKey ( KeyCode::A ) && direction != Direction::Right ) { direction = Direction::Left; }
    else if ( input->GetKey ( KeyCode::Escape ) ) { system ( "pause" ); GameTime::GetInstance ()->Tick (); }
}

void Snake::Move ( float deltaTime )
{
    //Get x and y of snake head position and modify them occording to current directon of snake
    float x = GetPosition ().X;
    float y = GetPosition ().Y;
    float bodyX = x;
    float bodyY = y;
    float snakeSpeedOverTime = ( speed * ( float ) deltaTime );

    //Modify position based on direction of snake
    switch ( direction )
    {
    case Direction::Up: y -= snakeSpeedOverTime; break;
    case Direction::Down: y += snakeSpeedOverTime; break;
    case Direction::Left: x -= snakeSpeedOverTime; break;
    case Direction::Right: x += snakeSpeedOverTime; break;
    }
    MoveSnakeBody ( bodyX, bodyY );
    SetPosition ( x, y );
}

void Snake::MoveSnakeBody ( float& bodyX, float& bodyY )
{
    for ( int i = 0; i < snakeBody.size (); ++i )
    {
        //Store this body parts position for next body part to reference before moveing this body part
        float tempX = snakeBody[ i ]->GetPosition ().X;
        float tempY = snakeBody[ i ]->GetPosition ().Y;
        snakeBody[ i ]->SetPosition ( bodyX, bodyY );
        bodyX = tempX;
        bodyY = tempY;
    }
}
