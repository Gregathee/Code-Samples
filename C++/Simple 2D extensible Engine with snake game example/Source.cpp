#include"GameEngine.h"
#include"SnakeGame.h"

int main ()
{
    SnakeGame* snakeGame = new SnakeGame();
    GameEngine::Instance()->StartEngine ();
    return 0;
}