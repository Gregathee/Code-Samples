#include "Transform.h"
#include"Vector2.h"
#define _USE_MATH_DEFINES
#include<math.h>

void Transform::Rotate ( float adjustment )
{
	rotation += adjustment;
	if ( rotation > 360 ) { rotation -= 360; }
}

float Transform::GetRotation () { return rotation; }

void Transform::SetRotation ( float newRotation )
{
	rotation = newRotation;
	if ( rotation > 360 ) { rotation -= 360; }
}

Vector2 Transform::Up2D ()
{
	float radians = rotation * ( M_PI / 180 );
	return Vector2 ( -sin ( radians ), cos ( radians ) );
}

Vector2 Transform::Right2D ()
{
	float radians = rotation * ( M_PI / 180 );
	return Vector2 ( cos ( radians ), sin ( radians ) );
}
