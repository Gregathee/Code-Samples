#pragma once
#include<iostream>
#include<string>
//I have plans to add linear algebra functions and other niceities
class Vector3;
class Vector2
{
public:
	float x = 0;
	float y = 0;

	Vector2 () {};
	Vector2 ( float x, float y );

	Vector2 Normalize ();

	float Magnitude ();

	static float AngleInRads ( Vector2 v1, Vector2 v2 );

	static float DotProduct ( Vector2 v1, Vector2 v2 );

	Vector2 operator- ( const Vector3& other );
	Vector2 operator- ( const Vector2& other );
	Vector2 operator+ ( const Vector3& other );
	Vector2 operator+ ( const Vector2& other );
	Vector2 operator* ( const float& other );
	Vector2 operator+= ( const Vector3& other );
	Vector2 operator-= ( const Vector3& other );
	Vector2 operator -= ( const Vector2& other );
	Vector2 operator += ( const Vector2& other );
	Vector2 operator* ( const Vector2& other );
	Vector2 operator*= ( const float& other );
	Vector2 operator*= ( const Vector2& other );
	Vector2 operator/= ( const float& other );
	Vector2 operator= ( const Vector3& other );
	bool operator== ( const Vector2& other );

	friend std::ostream& operator<< ( std::ostream& os, const Vector2& other )
	{
		os << "( " << other.x << ", " << other.y << " )";
		return os;
	}



	static Vector2 Zero ();
};