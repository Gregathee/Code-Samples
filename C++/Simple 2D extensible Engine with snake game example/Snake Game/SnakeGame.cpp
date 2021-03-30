#include "SnakeGame.h"

SnakeGame::SnakeGame () : GameObject ( false ) { engine->RegisterGameObject ( this ); }

void SnakeGame::Start () { InitializeSnakeGame (); }

void SnakeGame::Update ( float deltaTime ) { GameLogic (); }

void SnakeGame::InitializeSnakeGame ()
{
    CreateGameBorder ();
    CreateSnakeHead ();
    CreateFood ();
    PlaceFoodInRandomPosition ();
}

void SnakeGame::CreateGameBorder ()
{
    border = new GameObject ( GameEngine::Instance ()->GetGraphicsModule ()->CreateRectangle ( gameWindow->GetWidth () * 0.75f, gameWindow->GetHeight () * 0.75f ) );
    RectangleRenderShape* rectangle = static_cast< RectangleRenderShape* >( border->GetRenderShape () );
    border->SetPosition ( gameWindow->GetWidth () / 8.0f, gameWindow->GetHeight () / 8.0f );
    rectangle->SetFillColor ( Color ( 0, 0, 0 ) );
    rectangle->SetOutlineColor ( Color ( 255, 255, 255 ) );
    rectangle->SetOutlineThickness ( 1.0f );

    //Create quick references of the margins between the window border and the game border
    horizontalMargin = ( gameWindow->GetWidth () - rectangle->GetWidth () ) / 2;
    verticalMargin = ( gameWindow->GetHeight () - rectangle->GetHeight () ) / 2;
}

void SnakeGame::CreateSnakeHead ()
{
    snake = new Snake ( GameEngine::Instance ()->GetGraphicsModule ()->CreateCircle ( snakeRadius ), snakeRadius );
    snake->SetPosition ( gameWindow->GetWidth () / 2.0f, gameWindow->GetHeight () / 2.0f );
    snake->GetRenderShape ()->SetFillColor ( Color ( 0, 255, 0 ) );
}

void SnakeGame::CreateFood ()
{
    food = new GameObject ( GameEngine::Instance ()->GetGraphicsModule ()->CreateCircle ( snakeRadius ) );
    food->GetRenderShape ()->SetFillColor ( Color ( 255, 0, 0 ) );
}

void SnakeGame::PlaceFoodInRandomPosition ()
{
    //Get the range of random positions that fit inside the game border
    RectangleRenderShape* rectangle = static_cast< RectangleRenderShape* >( border->GetRenderShape () );
    CircleRenderShape* circle = static_cast< CircleRenderShape* >( food->GetRenderShape () );
    int xMod = ( int ) ( nearbyint ( rectangle->GetWidth () - ( circle->GetRadius () * 4 ) ) );
    int yMod = ( int ) ( nearbyint ( rectangle->GetHeight () - ( circle->GetRadius () * 4 ) ) );

    //Create random x and y and add adjustment value to shift the range of random positions to overlap the game border
    float xRand = ( float ) ( ( rand () % xMod ) ) + horizontalMargin + circle->GetRadius ();
    float yRand = ( float ) ( ( rand () % yMod ) ) + verticalMargin + circle->GetRadius ();
    food->SetPosition ( xRand, yRand );
}

bool SnakeGame::CircleColision ( CircleRenderShape*& shape1, CircleRenderShape*& shape2 )
{
    //define minimum distance that two circles could be apart without touching
    float combinedRadius = shape1->GetRadius () + shape2->GetRadius ();

    //use differences in x's and y's and use pythagorim theorm to get distances between the two circles centers
    float xDif = shape1->GetPosition ().X - shape2->GetPosition ().X;
    float yDif = shape1->GetPosition ().Y - shape2->GetPosition ().Y;
    float distance = sqrt ( pow ( xDif, 2.0f ) + pow ( yDif, 2.0f ) );

    //return if overlapping
    return distance < combinedRadius;
}

bool SnakeGame::CollidedWithWall ()
{
    float x = snake->GetPosition ().X;
    float y = snake->GetPosition ().Y;
    RectangleRenderShape* rectangle = static_cast< RectangleRenderShape* >( border->GetRenderShape () );
    CircleRenderShape* circle = static_cast< CircleRenderShape* >( snake->GetRenderShape () );
    float xMax = horizontalMargin + rectangle->GetWidth () - ( circle->GetRadius () * 2 );
    float yMax = verticalMargin + rectangle->GetHeight () - ( circle->GetRadius () * 2 );
    return x < horizontalMargin || x > xMax || y < verticalMargin || y > yMax;
}

void SnakeGame::GameOver ()
{
    snake->ClearBody ();
    snake->SetPosition ( gameWindow->GetWidth () / 2.0f, gameWindow->GetHeight () / 2.0f );
    PlaceFoodInRandomPosition ();
    framesBuffer = framesBufferSize;
}

void SnakeGame::GameLogic ()
{
    CircleRenderShape* headCircle = static_cast< CircleRenderShape* >( snake->GetRenderShape () );
    CircleRenderShape* foodCircle = static_cast< CircleRenderShape* >( food->GetRenderShape () );
    if ( CircleColision ( headCircle, foodCircle ) )
    {
        //Add body parts to buffer to be added 1 per frame
        framesBuffer += framesBufferSize;
        PlaceFoodInRandomPosition ();
    }
    //if body parts are done being added, check for collision with body parts
    else if ( framesBuffer <= 0 ) { if ( snake->CollidedWithBody () ) { GameOver (); } }
    else
    {
        //add one body part from buffer per frame
        for ( int i = 0; i < framesBuffer; ++i ) { snake->AddBody (); }
        framesBuffer--;
    }

    if ( CollidedWithWall () ) { GameOver (); }
}
