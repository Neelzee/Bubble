using Godot;
using System;

public partial class Shark : CharacterBody2D
{
	private CharacterBody2D _plr;
	private CharacterBody2D _body;
	private AnimatedSprite2D _animatedSprite;

	private float _smellDistance = 200;
	private double _speed = 150;

	private double _distance = 100;
	private float _topSpeed = 200;
	private Vector2 _origin;

	private bool _facingLeft = new Random().Next(0, 1) == 0;

	private bool _isTurning;
	
	private bool _isHunting;
	public override void _Ready()
	{
		_body = GetNode<CharacterBody2D>(".");
		if (_body == null) throw new NullReferenceException("CharacterBody2D is null");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_plr = GetNode<CharacterBody2D>("../Player");
		if (_plr == null) throw new NullReferenceException("Player is null");
		_origin = _body.GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		GD.Print(_plr.GlobalPosition);
		if (!_isHunting && _body.GlobalPosition.DistanceTo(_plr.GlobalPosition) <= _smellDistance)
		{
			_isHunting = true;
		}

		// Player is out of position :(
		if (_isHunting && _body.GlobalPosition.DistanceTo(_plr.GlobalPosition) > _distance)
		{
			_isTurning = false;
			_origin = _body.GlobalPosition;
		}

		if (_isHunting)
		{
			var velocity = _body.Velocity;
			if (velocity.Length() == 0)
			{
				velocity.X += (float) (_speed * delta);
				velocity.Normalized();
			}

			var playerRotation = _body.GlobalPosition.AngleToPoint(_plr.GlobalPosition);
			GD.Print(playerRotation);
			velocity *= (float) (_speed * delta);
			if (velocity.Length() >= _topSpeed)
			{
				velocity = velocity.Normalized() * _topSpeed;
			}
			velocity = velocity.Rotated(playerRotation * (float)delta);
			_body.Velocity = velocity;
			_animatedSprite.GlobalRotation = (float) Math.Atan2(velocity.Y, velocity.X);
			MoveAndSlide();
		}
		else
		{
			NotHunting(delta);
		}
	}

	private void NotHunting(double delta)
	{
		if (_body.Position.DistanceTo(_origin) >= _distance && !_isTurning)
		{
			_isTurning = true;
			_facingLeft = !_facingLeft;
			_speed = -_speed;
			if (_animatedSprite != null) _animatedSprite.FlipH = _facingLeft;
		}

		if (_isTurning && _body.Position.DistanceTo(_origin) <= _distance)
		{
			_isTurning = false;
		}
		var velocity = _body.Velocity;
		velocity.X += (float)(_speed * delta);
		if (velocity.Length() >= _topSpeed)
		{
			velocity = velocity.Normalized() * _topSpeed;
		}
		_body.Velocity = velocity;
		MoveAndSlide();
	}
}
