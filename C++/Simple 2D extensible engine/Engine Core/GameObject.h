#pragma once
#include"IGameObject.h"

/// <summary>
/// Used to be an empty vessel or base class for default functionality 
/// </summary>
class GameObject : public IGameObject
{
public:

	Transform transform;
	GameObject ();

	GameObject ( string newName ) { }
	
	GameObject (RenderShape* newShape );

	//used to prevent default constructor from being called so engine doesn't register this as a base class
	GameObject ( bool na );

	GameObject ( const GameObject& other );

	GameObject ( GameObject&& other ) noexcept;

	GameObject& operator=( const GameObject& other );

	GameObject& operator=( GameObject&& other ) noexcept;

	~GameObject ();

	//called on first frame of objects existence
	virtual void Start () { }

	//called once per frame
	virtual void Update ( float deltaTime ) { }

	virtual Transform& GetTransform ();

	Vector3 GetPosition ();

	void SetPosition ( Vector3 newPosition );

	void SetPosition ( float x, float y );

	void SetRenderShape ( RenderShape* newShape );

	RenderShape* GetRenderShape ();
protected:
	//for easy reference to engine members
	GameEngine* engine;
	Input*  input;
	GameWindow* gameWindow;
	RenderShape* renderShape = nullptr;

	//Assign engine references
	void Initialize ();
};

