#pragma once

//For now this just holds an EngineEventType
enum class EngineEventType{None, Closed};
class EngineEvent
{
public:
	EngineEventType Type = EngineEventType::None;
};