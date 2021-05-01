#pragma once
#include"GameObject.h"
#include"Snake.h"
//enum class Direction { Up, Down, Left, Right };
class SnakeGame: public GameObject
{
public:
    SnakeGame ();
    void Start () override;

    void Update ( float deltaTime ) override;

private:
    IGameObject* border = nullptr;
    IGameObject* food = nullptr;
    Snake* snake = nullptr;
    vector<IGameObject*> snakeBody;
    float snakeSpeed = 100;
    float snakeRadius = 60;
    float distanceForBodyColision = 10;
    int framesBufferSize = 5;
    int framesBuffer = 5;

    void InitializeSnakeGame ();

    void CreateGameBorder ();

    void CreateSnakeHead ();

    void CreateFood ();

    void PlaceFoodInRandomPosition ();

    bool CircleColision ( CircleRenderShape*& shape1, CircleRenderShape*& shape2 );

    bool CollidedWithWall ();

    void GameOver ();

    void GameLogic ();
};