#include "GameEngine.h"
#include "GameObject.h"


GameEngine* GameEngine::instance;

GameEngine::GameEngine ()
{
    input = new SFMLInput ();
    graphicsModule = new SFMLGraphicsModule ();
    gameWindow = graphicsModule->CreateWindow ();

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
    if ( gameObject->GetRenderShape () ) { gameObject->GetRenderShape ()->SetGameObject ( gameObject ); }
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
    multimap<float, shared_ptr<IGameObject>, greater<float>> drawnObjects;
    gameWindow->Clear ();
    FindDrawnObjects ( drawnObjects );
    DrawObjects ( drawnObjects );
    gameWindow->Display ();
}

void GameEngine::FindDrawnObjects ( multimap<float, shared_ptr<IGameObject>, greater<float>>& drawnObjects )
{
    for ( auto i : updatedObjects )
    {
        if ( i.second->GetRenderShape () )
        {
            drawnObjects.insert ( pair<float, shared_ptr<IGameObject>> ( i.second->GetTransform ().Position.z, i.second ) );
        }
    }
}

void GameEngine::DrawObjects ( multimap<float, shared_ptr<IGameObject>, greater<float>>& drawnObjects )
{
    for ( auto i : drawnObjects )
    {
        TranslateWorldPositionsToScreenCameraPosition ( i.second );
        if ( camera.GetFollowTarget () ) 
        {
            Vector2 newPos = TranslateScreenPositionRelativeToCameraRotation ( i.second->GetRenderShape ()->GetPosition () );
            i.second->GetRenderShape()->SetPosition(newPos);
            IGameObject* ptr = camera.GetFollowTarget ();
            if ( *i.second != ptr )
            {
                float cameraRotation = camera.GetFollowTarget ()->GetTransform ().GetRotation ();
                i.second->GetRenderShape()->SetRotation ( i.second->GetTransform ().GetRotation () + cameraRotation );
            }
        }
        gameWindow->Draw ( i.second->GetRenderShape () );
    }
}

void GameEngine::TranslateWorldPositionsToScreenCameraPosition (shared_ptr<IGameObject> gameObject)
{
    float x = gameObject->GetTransform ().Position.x;
    float y = gameObject->GetTransform ().Position.y;
    
    //translate world position to screen coordinates
    Vector2 drawPosition (  x + (windowWidth / 2), (y * -1) + windowHeight / 2  );

    //adjust relative to camera location
    if ( camera.GetFollowTarget () )
    {
        Vector3 adjustment = camera.GetFollowTarget ()->GetTransform ().Position;
        adjustment.x *= -1;
        drawPosition += adjustment;
    }
    gameObject->GetRenderShape ()->SetPosition ( drawPosition );
}

//rotate relative to player rotation then translate world positions to screen position
Vector2 GameEngine::TranslateScreenPositionRelativeToCameraRotation ( Vector2 screenPosition, bool reverse )
{
    //subtract screen center from point
    screenPosition.x -= windowWidth/2;
    screenPosition.y -= windowHeight/2;

    float angleInRadians = camera.GetFollowTarget ()->GetTransform ().GetRotation () * ( M_PI / 180 );
    if ( reverse ) angleInRadians *= -1;

    //Get coordinates after rotaion
    float x = ( screenPosition.x * cos ( angleInRadians ) ) - ( screenPosition.y * sin ( angleInRadians ) );
    float y = ( screenPosition.x * sin ( angleInRadians ) ) + ( screenPosition.y * cos ( angleInRadians ) );

    //add screen center back to point
    Vector2 newPos ( x + windowWidth/2, y + windowHeight/2 );
    return newPos;
}
