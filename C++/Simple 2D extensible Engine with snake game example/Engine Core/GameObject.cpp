#include "GameObject.h"


GameObject::GameObject () 
{
	Initialize (); 
	engine->RegisterGameObject ( this );
}

GameObject::GameObject ( RenderShape * newShape )
{
	SetRenderShape ( newShape );
	Initialize ();
	engine->RegisterGameObject ( this );
}

GameObject::GameObject ( bool na ) { Initialize (); }

GameObject::GameObject ( const GameObject& other )
{
	if(other.renderShape) renderShape = other.renderShape->Clone ();
}

GameObject& GameObject::operator=( const GameObject& other ) 
{
	if ( this == &other ) return *this;
	delete renderShape;
	if(other.renderShape) renderShape = other.renderShape->Clone ();
	return *this; 
}

GameObject::GameObject ( GameObject&& other ) noexcept
{
	renderShape = other.renderShape;
	other.renderShape = nullptr;
}

GameObject::~GameObject () { delete renderShape; }

GameObject& GameObject::operator=( GameObject&& other ) noexcept 
{
	if ( this == &other ) return *this;
	delete renderShape;
	renderShape = other.renderShape;
	other.renderShape = nullptr;
}

void GameObject::Initialize ()
{
	engine = GameEngine::Instance ();
	input = engine->GetInput ();
	gameWindow = engine->GetGameWindow ();
}

Vector2 GameObject::GetPosition () { return position; }

void GameObject::SetPosition ( Vector2 newPosition )
{
	position = newPosition;
	if(renderShape) renderShape->SetPosition ( newPosition );
}

void GameObject::SetPosition ( float x, float y )
{
	position.X = x;
	position.Y = y;
	if ( renderShape ) renderShape->SetPosition ( position );
}

void GameObject::SetRenderShape ( RenderShape* newShape ) { renderShape = newShape; }

RenderShape* GameObject::GetRenderShape (){ return renderShape; }