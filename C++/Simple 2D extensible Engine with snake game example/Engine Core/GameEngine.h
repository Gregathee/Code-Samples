#pragma once
#include<iostream>
#include<map>
#include<math.h>
#include<random>
#include<time.h>
#include"SFMLInput.h"
#include"SFMLGraphicsModule.h"
#include"GameTime.h"

using namespace std;

class IGameObject;
class GameEngine
{
public:

    static GameEngine* Instance ();

    Input* GetInput () { return input; }

    GameWindow* GetGameWindow ();

    GraphicsModule* GetGraphicsModule ();

    //Registers game objects to be started and updated
    void RegisterGameObject ( IGameObject* gameObject );

    void DestroyGameObject ( int gameObjectID );
    
    void StartEngine ();

private:
    Input* input;
    static GameEngine* instance;
    map <int, shared_ptr<IGameObject>> updatedObjects;
    //objects to be deleted before update
    vector<int> objectsToBeDeleted;
    //objects to be added before update
    vector<shared_ptr<IGameObject>> objectsToBeAdded;
    GraphicsModule* graphicsModule;
	GameWindow* gameWindow;
    int nextGameObjectID = 0;
    float deltaTime = 0;

    GameEngine ();

    void DeleteObjects ();

    void AddObjects ();

    void Draw ();
};