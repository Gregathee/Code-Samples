#pragma once
#include<iostream>
#include<map>
#define _USE_MATH_DEFINES
#include<math.h>
#include<random>
#include<time.h>
#include"SFMLInput.h"
#include"SFMLGraphicsModule.h"
#include"GameTime.h"
#include<string>
#include"Camera.h"

using std::map;
using std::multimap;
using std::vector;
using std::shared_ptr;
using std::string;
using std::pair;
using std::greater;
using std::cout;
using std::endl;

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

    void SetCameraFollowTarget ( IGameObject* gameObject ) { camera.SetFollowTarget ( gameObject ); }

    IGameObject* GetCameraFollowTarget () { return camera.GetFollowTarget (); }

    Vector2 TranslateScreenPositionRelativeToCameraRotation ( Vector2 shape, bool reverse = false );

private:
    Input* input;
    static GameEngine* instance;
    GraphicsModule* graphicsModule;
    GameWindow* gameWindow;
    Camera camera;
    map <int, shared_ptr<IGameObject>> updatedObjects;
    //objects to be deleted before update
    vector<int> objectsToBeDeleted;
    //objects to be added before update
    vector<shared_ptr<IGameObject>> objectsToBeAdded;
    int nextGameObjectID = 0;
    float deltaTime = 0;
    unsigned int windowWidth = 1920;
    unsigned int windowHeight = 1080;

    GameEngine ();

    void DeleteObjects ();

    void AddObjects ();

    void Draw ();

    void FindDrawnObjects ( multimap<float, shared_ptr<IGameObject>, greater<float>>& drawnObjects );

    void DrawObjects ( multimap<float, shared_ptr<IGameObject>, greater<float>>& drawnObjects );

    void TranslateWorldPositionsToScreenCameraPosition ( shared_ptr<IGameObject> gameObject );
};