#pragma once
#include"GameObject.h"
enum class Direction { Up, Down, Left, Right };
class Snake: public GameObject
{
public:
    Snake ();

    Snake ( RenderShape* newRenderShape, float radius );

    void Update ( float deltaTime ) override;

    void AddBody ();

    bool CollidedWithBody ();

    void ClearBody ();

private:
    vector<IGameObject*> snakeBody;
    Direction direction = Direction::Up;
    float speed = 100;
    float snakeRadius = 30;
    float distanceForBodyColision = 10;
    int framesBufferSize = 5;
    int framesBuffer = 5;

    void ProcessInput ();

    void Move ( float deltaTime );

    void MoveSnakeBody ( float& bodyX, float& bodyY );
};