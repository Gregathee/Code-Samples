#include "Vector2.h"
#include "Vector3.h"
#include<math.h>

Vector2::Vector2 ( float x, float y ) { this->x = x; this->y = y; }

Vector2 Vector2::Normalize ()
{
	float magnitude = Magnitude ();
	if ( magnitude > 0 )
	{
		x /= magnitude;
		y /= magnitude;
	}
	return *this;
}

float Vector2::Magnitude () { return sqrt ( pow ( x, 2.0f ) + pow ( y, 2.0f ) ); }

float Vector2::AngleInRads ( Vector2 v1, Vector2 v2 )
{
	return  acos(DotProduct ( v1, v2 ) / ( v1.Magnitude () * v2.Magnitude () ));
}

float Vector2::DotProduct ( Vector2 v1, Vector2 v2 )
{
	Vector2 product = v1 * v2;
	return product.x + product.y;
}

Vector2 Vector2::operator-( const Vector3& other )
{
	Vector2 v ( *this );
	v.x -= other.x;
	v.y -= other.y;
	return v;
}

Vector2 Vector2::operator-( const Vector2& other )
{
	Vector2 v ( *this );
	v.x -= other.x;
	v.y -= other.y;
	return v;
}

Vector2 Vector2::operator+( const Vector3& other )
{
	Vector2 v ( *this );
	v.x += other.x;
	v.y += other.y;
	return v;
}

Vector2 Vector2::operator+( const Vector2& other )
{
	Vector2 v ( *this );
	v.x += other.x;
	v.y += other.y;
	return v;
}

Vector2 Vector2::operator*( const float& other )
{
	Vector2 v ( *this );
	v.x *= other;
	v.y *= other;
	return v;
}

Vector2 Vector2::operator+=( const Vector3& other )
{
	x += other.x;
	y += other.y;
	return *this;
}

Vector2 Vector2::operator-=( const Vector3& other )
{
	x -= other.x;
	y -= other.y;
	return *this;
}

Vector2 Vector2::operator-=(const Vector2& other )
{
	x -= other.x;
	y -= other.y;
	return *this;
}

Vector2 Vector2::operator+=( const Vector2& other )
{
	x += other.x;
	y += other.y;
	return *this;
}

Vector2 Vector2::operator*( const Vector2& other )
{
	Vector2 result ( x, y );
	result.x *= other.x;
	result.y *= other.y;
	return result;
}

Vector2 Vector2::operator*=( const float& other )
{
	x *= other;
	y *= other;
	return *this;
}

Vector2 Vector2::operator*=( const Vector2& other )
{
	x *= other.x;
	y *= other.y;
	return *this;
}

Vector2 Vector2::operator/=( const float& other )
{
	x /= other;
	y /= other;
	return *this;
}

Vector2 Vector2::operator=( const Vector3& other )
{
	x = other.x;
	y = other.y;
	return *this;
}

bool Vector2::operator==( const Vector2& other )
{
	return ( x == other.x && y == other.y );
}



Vector2 Vector2::Zero () { return Vector2 ( 0, 0 ); }

