#pragma once
#include"Color.h"
#include"Vector2.h"
#include<iostream>

//Interface for objects that are rendered on screen
class IGameObject;
class RenderShape
{
public:
	bool debug = false;
	virtual RenderShape* Clone () = 0;

	virtual void SetFillColor ( Color newColor ) = 0;

	virtual void SetPosition ( Vector2& newPosition ) = 0;

	virtual Vector2 GetPosition () = 0;

	IGameObject* GetGameObject () { return gameObject; }

	void SetGameObject ( IGameObject* newGameObject ) { gameObject = newGameObject; }

	virtual void SetRotation ( float newRotation ) = 0;

	virtual float GetRotation () = 0;

protected:
	Color* color;
	Vector2 position;
	IGameObject* gameObject;
	float rotation = 0;
};