using Godot;
using System;

public partial class BobMovement3 : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float MaxSpeed = 1500.0f;
	public const float Friction = 600f;
	public const int radiusMax = 500;
	public const int radiusMin = 30;
	public const float bounceVar = 0.7f;
	
	private Vector2 InputDirection = Vector2.Zero;
	
	private Vector2 GetInpDir(){
		Vector2 relativeVector = Vector2.Zero;
		if(Input.IsActionPressed("cursor_left")){
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 playerPosition = this.GlobalPosition;
			float dist = 0;
			relativeVector = new Vector2(playerPosition.X - mousePosition.X, playerPosition.Y - mousePosition.Y);
			dist = Mathf.Sqrt(Mathf.Pow(relativeVector.X, 2) + Mathf.Pow(relativeVector.Y, 2));
			//relativeVector = relativeVector.Normalized();
			
			if(dist < radiusMax){
				float scale = Mathf.Min(radiusMax - dist, radiusMax - radiusMin)/(radiusMax - radiusMin);
				relativeVector.X = relativeVector.X;
				relativeVector.Y = relativeVector.Y;
				relativeVector = relativeVector.Normalized() * scale;
			}
			else{
				relativeVector = Vector2.Zero;
			}
		}
		return relativeVector;
	}

	public void UpdateVelocity(double delta){
		Vector2 InputDirection = GetInpDir();
		Vector2 vel = Vector2.Zero;
		Vector2 new_vel = GetVelocity();
		float scaleSpeedY = Mathf.Abs(InputDirection.Y);
		float scaleSpeedX = Mathf.Abs(InputDirection.X);
		
		
		vel = InputDirection * MaxSpeed;

		if(InputDirection.X != 0){
			new_vel.X = (float)Mathf.MoveToward(Velocity.X, vel.X, delta*Speed*scaleSpeedX);
		}
		else{
			new_vel.X = (float)Mathf.MoveToward(Velocity.X, 0, delta*(Friction/MaxSpeed)*Speed);
		}
		if(InputDirection.Y != 0){
			new_vel.Y = (float)Mathf.MoveToward(Velocity.Y, vel.Y, delta*Speed*scaleSpeedY);
		}
		else{
			new_vel.Y = (float)Mathf.MoveToward(Velocity.Y, 0, delta*(Friction/MaxSpeed)*Speed);
		}
		
		SetVelocity(new_vel);
	}
	
	public override void _PhysicsProcess(double delta){
		// We save velocity before collision, to bounce on it
		Vector2 bounceVelocity = GetVelocity() * bounceVar;
		MoveAndSlide();
		if (GetSlideCollisionCount() > 0){
			var collision = GetSlideCollision(0);
			if (collision != null){
				Velocity = bounceVelocity.Bounce(collision.GetNormal());
			}
			
		}
		UpdateVelocity(delta);
	}

	public void OnBodyEnteredKillZone(PhysicsBody2D body){
		GetTree().CallDeferred("reload_current_scene");
	}
};
