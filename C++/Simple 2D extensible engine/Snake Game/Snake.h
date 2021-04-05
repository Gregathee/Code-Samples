#pragma once
#include"GameObject.h"
enum class Direction { Up, Down, Left, Right };
class Snake: public GameObject
{
public:

    float degreesToRotate = 0;

    Direction direction = Direction::Up;
    Snake ();

    Snake ( RenderShape* newRenderShape, float radius );

    void Update ( float deltaTime ) override;

    void AddBody ();

    bool CollidedWithBody ();

    void ClearBody ();

private:
    vector<IGameObject*> snakeBody;
    float speed = 400;
    float snakeRadius = 60;
    float distanceForBodyColision = 10;
    int framesBufferSize = 5;
    int framesBuffer = 5;
    float degreesPerFrame = 2;

    void ProcessInput ();

    void Move ( float deltaTime );

    void MoveSnakeBody ( float& bodyX, float& bodyY );

    void Rotate ();
};