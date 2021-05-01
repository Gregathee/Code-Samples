#include <SFML/Graphics.hpp>
#include<iostream>
#include<math.h>
#include<vector>
#include<random>
#include<time.h>

using namespace sf;
using namespace std;

enum class Direction { Up, Down, Left, Right };

//Struct used to avoid global variables and packaging commonly passed variables
struct Game
{
    RenderWindow window;
    RectangleShape* border;
    CircleShape* food;
    CircleShape* snakeHead;
    vector<CircleShape*> snakeBody;
    vector<Shape*> shapes;
    Clock gameClock = Clock ();
    Direction direction = Direction::Up;
    float snakeSpeed = 20;
    float snakeRadius = 30;
    float distanceForBodyColision = 2;
    int denominator = 100000;
    float verticalMargin;
    float horizontalMargin;
    int framesBufferSize = 5;
    int framesBuffer = 0;
}; 

void Initialize (Game& game);
void ProcessInput (Game& game);
void Draw (Game& game);
void GameLogic (Game& game);

int main ()
{
    srand ( time ( NULL ) );
    Game game;
    Initialize (game);
    while ( game.window.isOpen () )
    {
        Event event;
        while ( game.window.pollEvent ( event ) ) { if ( event.type == Event::Closed ) { game.window.close (); } }
        ProcessInput (game);
        GameLogic (game);
        game.gameClock.restart ();
        Draw (game);
    }
    return 0;
}

void PlaceFoodInRandomPosition (Game& game)
{
    //Get therange of random positions that fit inside the game border
    int xMod = ( int ) ( nearbyint ( game.border->getSize ().x - ( game.food->getRadius () * 4 ) ));
    int yMod = ( int ) ( nearbyint ( game.border->getSize ().y - ( game.food->getRadius () * 4 ) ) );

    //Create random x and y and add adjustment value to shift the range of random positions to overlap the game border
    float xRand = (float)(( rand () % xMod)) + game.verticalMargin + game.food->getRadius();
    float yRand = (float)(( rand () % yMod)) + game.horizontalMargin + game.food->getRadius();
    game.food->setPosition ( xRand, yRand );
}

void Initialize (Game& game)
{
    int windowWidth = 1920;
    int windowHeight = 1080;

    //Create window
    if ( !game.window.isOpen())
    {
        game.window.create ( sf::VideoMode ( windowWidth, windowHeight ), "Snake" );
        game.window.setVerticalSyncEnabled ( true );
    }

    //Create game border
    game.border = new RectangleShape ( Vector2f ( windowWidth * 0.75f, windowHeight * 0.75f ) );
    game.border->setPosition ( windowWidth / 8.0f, windowHeight / 8.0f );
    game.border->setFillColor ( sf::Color::Black );
    game.border->setOutlineColor ( sf::Color::White );
    game.border->setOutlineThickness ( 1.0f );

    //Create snake head sprite in middle of the game border
    game.snakeHead = new CircleShape ( game.snakeRadius );
    game.snakeHead->setPosition ( windowWidth / 2.0f, windowHeight / 2.0f );
    game.snakeHead->setFillColor ( Color::Green );

    //Create food sprite
    game.food = new CircleShape ( game.snakeRadius );
    game.food->setFillColor ( Color::Red );

    //Add shapes to list to be drawn each frame
    game.shapes.push_back ( game.border );
    game.shapes.push_back ( game.snakeHead );
    game.shapes.push_back ( game.food );

    //Create quick references of the margins between the window border and the game border
    game.verticalMargin = ( game.window.getSize().x - game.border->getSize().x ) /2;
    game.horizontalMargin = ( game.window.getSize ().y - game.border->getSize ().y ) / 2;

    PlaceFoodInRandomPosition ( game );
}

void ProcessInput (Game& game)
{
    if        (Keyboard::isKeyPressed ( Keyboard::W ) ) { game.direction = Direction::Up; }
    else if (Keyboard::isKeyPressed ( Keyboard::D ) ) { game.direction = Direction::Right; }
    else if (Keyboard::isKeyPressed ( Keyboard::S ) ) { game.direction = Direction::Down; }
    else if (Keyboard::isKeyPressed ( Keyboard::A ) ) { game.direction = Direction::Left; }
    else if ( Keyboard::isKeyPressed ( Keyboard::Escape ) ) { system ( "pause" ); game.gameClock.restart (); }
}

void MoveSnake (Game& game)
{
    //create speed variable to smooth out movement independent of frame rate
    float snakeSpeedOverTime = ( game.snakeSpeed * ( float ) game.gameClock.getElapsedTime ().asMicroseconds () / game.denominator );
    
    //Get x and y of snake head position and modify them occording to current directon of snake
    float x = game.snakeHead->getPosition ().x;
    float y = game.snakeHead->getPosition ().y;
    float bodyX = x;
    float bodyY = y;
    switch ( game.direction )
    {
    case Direction::Up:
        y -= snakeSpeedOverTime;
        break;
    case Direction::Down:
        y += snakeSpeedOverTime;
        break;
    case Direction::Left:
        x -= snakeSpeedOverTime;
        break;
    case Direction::Right:
        x += snakeSpeedOverTime;
        break;
    }
    //Move each body part to follow the body part in front of it
    for ( int i = 0; i < game.snakeBody.size(); ++i)
    {
        //Store this body parts position for next body part to reference before moveing this body part
        float tempX = game.snakeBody[ i ]->getPosition ().x;
        float tempY = game.snakeBody[ i ]->getPosition ().y;
        game.snakeBody[ i ]->setPosition ( bodyX, bodyY );
        bodyX = tempX;
        bodyY = tempY;
    }
    game.snakeHead->setPosition ( x, y );
}

void Draw (Game& game)
{
    game.window.clear ();
    for ( auto i : game.shapes ) { game.window.draw ( *i ); }
    game.window.display ();
}


bool CircleColision ( CircleShape*& shape1, CircleShape*& shape2 )
{
    //define minimum distance that two circles could be apart without touching
    float combinedRadius = shape1->getRadius () + shape2->getRadius ();

    //use differences in x's and y's and use pythagorim theorm to get distances between the two circles centers
    float xDif = shape1->getPosition ().x - shape2->getPosition ().x;
    float yDif = shape1->getPosition ().y - shape2->getPosition ().y;
    float distance = sqrt ( pow ( xDif, 2.0f ) + pow ( yDif, 2.0f ) );

    //return if overlapping
    return distance < combinedRadius;
}

bool CollidedWithBody (Game& game)
{
    for ( int i = 0; i < game.snakeBody.size (); i++ )
    {
        //use differences in x's and y's and use pythagorim theorm to get distances between the two circles centers
        float xDif = game.snakeHead->getPosition ().x - game.snakeBody[i]->getPosition ().x;
        float yDif = game.snakeHead->getPosition ().y - game.snakeBody[ i ]->getPosition ().y;
        float distance = sqrt ( pow ( xDif, 2.0f ) + pow ( yDif, 2.0f ) );

        //because of the implementation of the snake body, body parts and head naturally overlap so the threshold is used to determine collision
        //return true if distances is within set threshold to determine "collision"
        if ( distance < game.distanceForBodyColision ) { return true; }
    }
    return false;
}

bool CollidedWithWall ( Game& game )
{
    int x = game.snakeHead->getPosition ().x;
    int y = game.snakeHead->getPosition ().y;
    int xMax = game.verticalMargin + game.border->getSize ().x - ( game.snakeHead->getRadius () * 2 );
    int yMax = game.horizontalMargin + game.border->getSize ().y - ( game.snakeHead->getRadius () * 2 );
    return OutsideRange ( x, game.verticalMargin, xMax ) || OutsideRange ( y, game.horizontalMargin, yMax );
}

void GameOver ( Game& game )
{
    //clean up shape pointers and reset vectors and reinitialize game
    for ( auto i : game.shapes ) { delete( i ); }
    game.shapes.clear ();
    game.snakeBody.clear ();
    Initialize ( game );
}

void AddBody ( Game& game )
{
    //create new body part
    CircleShape* newBody = new CircleShape ( game.snakeRadius );
    Color bodyColor ( 0, 128, 0, 255 );
    newBody->setFillColor ( bodyColor );

    //if snake has a tail, place new body part at position of tail, else place at head.
    if ( game.snakeBody.size () > 0 ) { newBody->setPosition ( game.snakeBody[ game.snakeBody.size () - 1 ]->getPosition ().x, game.snakeBody[ game.snakeBody.size () - 1 ]->getPosition ().y ); }
    else { newBody->setPosition ( game.snakeHead->getPosition ().x, game.snakeHead->getPosition ().y ); }

    //add new body part to vectors to be drawn and repositioned each frame
    game.snakeBody.push_back ( newBody );
    game.shapes.push_back ( newBody );
}

//return true if target is outside of given range
bool OutsideRange ( float target, float min, float max ) { return ( target < min || target > max ); }

void GameLogic ( Game& game )
{
    MoveSnake ( game );

    if ( CircleColision ( game.snakeHead, game.food ) )
    {
        //Add body parts to buffer to be added 1 per frame
        game.framesBuffer += game.framesBufferSize;
        PlaceFoodInRandomPosition ( game );
    }
    //if body parts are done being added, check for collision with body parts
    else if ( game.framesBuffer <= 0 ) { if ( CollidedWithBody ( game ) ) { GameOver (game); } }
    else 
    {
        //add one body part from buffer per frame
        for ( int i = 0; i < game.framesBuffer; ++i ) { AddBody ( game ); }
        game.framesBuffer--; 
    }

    if ( CollidedWithWall ( game ) ) { GameOver (game); }
}