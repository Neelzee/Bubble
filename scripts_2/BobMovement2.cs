using Godot;
using System;

public partial class BobMovement2 : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float MaxSpeed = 1500.0f;
	public const float Friction = 600;
	
	private Vector2 InputDirection = Vector2.Zero;
	
	private Vector2 GetInpDir(){
		Vector2 relativeVector = Vector2.Zero;
		if(Input.IsActionPressed("cursor_left")){
			Vector2 mousePosition = GetViewport().GetMousePosition();
			Vector2 playerPosition = this.GlobalPosition;
			GD.Print("Mouse_pos: ", mousePosition, " playerPosition: ", playerPosition);
			relativeVector = new Vector2(playerPosition.X - mousePosition.X, playerPosition.Y - mousePosition.Y);
		}
		return relativeVector.Normalized();
	}

	public void UpdateVelocity(double delta){
		Vector2 InputDirection = GetInpDir();
		Vector2 vel = Vector2.Zero;
		Vector2 new_vel = GetVelocity();
		
		vel = InputDirection.Normalized() * MaxSpeed;
		
		if(InputDirection.X != 0){
			new_vel.X = (float)Mathf.MoveToward(Velocity.X, vel.X, delta*Speed);
		}
		else{
			new_vel.X = (float)Mathf.MoveToward(Velocity.X, 0, delta*Speed*(Friction/MaxSpeed));
		}
		if(InputDirection.Y != 0){
			new_vel.Y = (float)Mathf.MoveToward(Velocity.Y, vel.Y, delta*Speed);
		}
		else{
			new_vel.Y = (float)Mathf.MoveToward(Velocity.Y, 0, delta*(Friction/MaxSpeed)*Speed);
		}
		//GD.Print("Velocity: ", Velocity);
		SetVelocity(new_vel);
	}
	
	public override void _PhysicsProcess(double delta){
		UpdateVelocity(delta);

		MoveAndSlide();
	}
};
