#pragma once
#include"Vector3.h"

class Transform
{
public:
	Transform(){}
	Vector3 Position;
	void Rotate ( float adjustment );
	float GetRotation ();
	void SetRotation ( float newRotation );

	Vector2 Up2D ();

	Vector2 Right2D ();
private:
	float rotation = 0;
};