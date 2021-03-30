#pragma once

//I have plans to add linear algebra functions and other niceities
class Vector2
{
public:
	float X = 0;
	float Y = 0;

	Vector2 () {};
	Vector2 ( float x, float y );

	static Vector2 Zero ();
};