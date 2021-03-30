#include "GameEngine.h"
#include "GameObject.h"


GameEngine* GameEngine::instance;

GameEngine::GameEngine ()
{
    input = new SFMLInput ();
    graphicsModule = new SFMLGraphicsModule ();
    gameWindow = graphicsModule->CreateWindow ();
    unsigned int windowWidth = 1920 / 2;
    unsigned int windowHeight = 1080 / 2;

    //Create window
    gameWindow->Create ( windowWidth, windowHeight, "Snake" );
    gameWindow->EnableVerticalSync ( true );
}

GameEngine* GameEngine::Instance ()
{
    if ( !instance ) { instance = new GameEngine; }
    return instance;
}

GameWindow* GameEngine::GetGameWindow (){ return gameWindow;}

GraphicsModule* GameEngine::GetGraphicsModule () { return graphicsModule; }

void GameEngine::RegisterGameObject ( IGameObject* gameObject )
{
    gameObject->ID = ++nextGameObjectID;
    //Defer objects to be added before update is called
    objectsToBeAdded.push_back ( shared_ptr<IGameObject> ( gameObject ) );
}

void GameEngine::DestroyGameObject ( int gameObjectID )
{
    //defer objects to be deleted before update is called
    objectsToBeDeleted.push_back ( gameObjectID );
}

void GameEngine::DeleteObjects ()
{
    for ( auto id : objectsToBeDeleted )
    {
        auto updatedGameObjectsItr = updatedObjects.find ( id );
        if ( updatedGameObjectsItr == updatedObjects.end () ) return;
        updatedObjects.erase ( updatedGameObjectsItr );
    }
    objectsToBeDeleted.clear ();
}

void GameEngine::AddObjects ()
{
    for ( int i = 0; i < objectsToBeAdded.size(); i++ )
    {
        updatedObjects.insert ( pair<int, shared_ptr<IGameObject>> ( objectsToBeAdded[i]->ID, shared_ptr<IGameObject> ( objectsToBeAdded[ i ] ) ) );
        objectsToBeAdded[ i ]->Start ();
    }
    objectsToBeAdded.clear ();
}

void GameEngine::StartEngine ()
{
    srand ( ( unsigned int ) time ( NULL ) );
    while ( gameWindow->IsOpen () )
    {
        DeleteObjects ();
        AddObjects ();
        input->UpdateKeys ();
        deltaTime = GameTime::GetInstance ()->Tick ();
        EngineEvent event;
        while ( gameWindow->PollEvent ( event ) ) { if ( event.Type == EngineEventType::Closed ) { gameWindow->Close (); } }
        for ( pair<int, shared_ptr<IGameObject>> gameObjectPair : updatedObjects )
        {
            if(gameObjectPair.second) gameObjectPair.second->Update ( deltaTime );
        }
        Draw ();
    }
}

void GameEngine::Draw ()
{
    gameWindow->Clear ();
    for ( auto i : updatedObjects ) 
    {
        if ( i.second->GetRenderShape () ) { gameWindow->Draw ( i.second->GetRenderShape () ); }
    }
    gameWindow->Display ();
}
