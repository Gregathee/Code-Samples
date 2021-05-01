#include "Vector3.h"
#include "Vector2.h"

Vector2 Vector3::operator-( Vector2 other )
{
    Vector2 v2 ( x, y );
    v2.x = x - other.x;
    v2.y = y - other.y;
    return v2;
}
