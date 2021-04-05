#pragma once
#include"IGameObject.h"

class Camera
{
public:
	void SetFollowTarget ( IGameObject* target ) { followTarget = target; }
	IGameObject* GetFollowTarget () { return followTarget; }
private:
	IGameObject* followTarget;
};