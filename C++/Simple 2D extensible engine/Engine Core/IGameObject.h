#pragma once
#include"GameEngine.h"
#include"Transform.h"

//GameObject interface
class IGameObject
{
public:
	std::string name = "Default Game Object";
	int ID = 0;

	//called on first frame of objects existence
	virtual void Start () = 0;

	//called once per frame
	virtual void Update ( float deltaTime ) = 0;

	virtual Transform& GetTransform () = 0;

	virtual Vector3 GetPosition () = 0;

	virtual void SetPosition ( Vector3 newPosition ) = 0;

	virtual void SetPosition ( float x, float y ) = 0;

	virtual void SetRenderShape ( RenderShape* newShape ) = 0;

	virtual RenderShape* GetRenderShape () = 0;

	bool operator== ( IGameObject*& other ) { return ID == other->ID; }
	bool operator== ( std::shared_ptr<IGameObject>& other ) { return ID == other->ID; }
	bool operator!= ( IGameObject*& other ) { return ID != other->ID; }
	bool operator!= ( std::shared_ptr<IGameObject>& other ) { return ID != other->ID; }
};

