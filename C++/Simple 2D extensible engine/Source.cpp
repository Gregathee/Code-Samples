#include"GameEngine.h"
#include"SnakeGame.h"
#include"NewGame.h"

int main ()
{
    //SnakeGame snakeGame;;
    NewGame game;
    GameEngine::Instance()->StartEngine ();
    return 0;
}