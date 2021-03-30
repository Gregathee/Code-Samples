#pragma once
#include"GameEngine.h"

//GameObject interface
class IGameObject
{
public:
	string name = "Default Game Object";
	int ID = 0;

	//called on first frame of objects existence
	virtual void Start () = 0;

	//called once per frame
	virtual void Update ( float deltaTime ) = 0;

	virtual Vector2 GetPosition () = 0;

	virtual void SetPosition ( Vector2 newPosition ) = 0;

	virtual void SetPosition ( float x, float y ) = 0;

	virtual void SetRenderShape ( RenderShape* newShape ) = 0;

	virtual RenderShape* GetRenderShape () = 0;
};

