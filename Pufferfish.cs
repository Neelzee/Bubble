using Godot;
using System;

public partial class Pufferfish : CharacterBody2D
{
	private CharacterBody2D _body;
	private double _distance = 100;
	private double _speed = 100;
	private float _topSpeed = 100;
	private Vector2 _origin;
	private AnimatedSprite2D _animatedSprite;

	private bool _facingLeft = new Random().Next(0, 1) == 0;

	private bool _isTurning;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_body = GetNode<CharacterBody2D>(".");
		if (_body == null) throw new NullReferenceException("CharacterBody2D is null");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_origin = _body.Position;
		if (_animatedSprite != null) _animatedSprite.FlipH = _facingLeft;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
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
