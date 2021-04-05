#pragma once
#include<iostream>
class Vector2;
class Vector3
{
public:
	Vector3(){}
	Vector3 ( float newX, float newY, float newZ )
	{
		this->x = newX;
		this->y = newY;
		this->z = newZ;
	}

	float Magnitude ()
	{
		return sqrt ( pow ( x, 2.0f ) + pow ( y,  2.0f ) + pow(z, 2.0f ));
	}

	Vector3 Normalize ()
	{
		float magnitude = Magnitude ();
		if ( magnitude > 0 )
		{
			x /= magnitude;
			y /= magnitude;
			z /= magnitude;
		}
		return *this;
	}

	Vector3 operator+ ( Vector3& other )
	{
		Vector3 newVector ( *this );
		newVector.x += other.x;
		newVector.y += other.y;
		newVector.z += other.z;
		return newVector;
	}

	Vector3 operator- ( Vector3& other )
	{
		Vector3 newVector ( *this );
		newVector.x -= other.x;
		newVector.y -= other.y;
		newVector.z -= other.z;
		return newVector;
	}

	Vector2 operator- ( Vector2 other );

	Vector3 operator-= ( Vector3& other )
	{
		x -= other.x;
		y -= other.y;
		z -= other.z;
		return *this;
	}

	Vector3& operator+= ( Vector3 other )
	{
		x += other.x;
		y += other.y;
		z += other.z;
		return *this;
	}

	Vector3 operator* ( float scalar )
	{
		Vector3 newVector( *this );
		newVector.x *= scalar;
		newVector.y *= scalar;
		newVector.z *= scalar;
		return newVector;
	}

	Vector3 operator*= ( float scalar )
	{
		x *= scalar;
		y *= scalar;
		z *= scalar;
		return *this;
	}

	Vector3 operator*= ( Vector3  other )
	{
		x *= other.x;
		y *= other.y;
		z *= other.z;
		return *this;
	}

	friend std::ostream& operator<< ( std::ostream& os, const Vector3& other )
	{
		os << "( " <<  other.x  << ", " <<  other.y << ", " << other.z << " )";
		return os;
	}

	float x = 0;
	float y = 0;
	float z = 0;
};