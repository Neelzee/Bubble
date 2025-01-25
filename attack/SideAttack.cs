using Godot;
using System;

public partial class SideAttack : Node2D
{
	private AnimatedSprite2D _warningSprite;
	private CharacterBody2D _attacker;
	private CharacterBody2D _plr;
	[Export] public bool IsGoingRight { get; set; } = true;
	[Export] public SpriteFrames Frames { get; set; }

	private bool _isInAttackRange;
	private float _warningTime = 5;
	private float _warningToggle = 0.5f;
	private double _warningToggleTick;
	private double _tick;
	private int _distance = 300;
	private float _speed = 150;
	private float _topSpeed = 4000;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_warningSprite = GetNode<AnimatedSprite2D>("./AttackWarning");
		_attacker = GetNode<CharacterBody2D>("./AttackPattern");
		_plr = GetNode<CharacterBody2D>("../Bob");
		if (_warningSprite == null || _attacker == null) throw new NullReferenceException(
			"Couldn't find AttackWarning or AttackPattern"
		);
		_warningSprite.Visible = false;
		_warningSprite.FlipH = !IsGoingRight;
		var attackerSprite = _attacker.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (Frames != null)
		{
			attackerSprite.SpriteFrames = Frames;
		}
		attackerSprite.FlipH = !IsGoingRight;
		if (IsGoingRight) return;
		_attacker.GlobalPosition = new Vector2(
			_attacker.GlobalPosition.X + 1000,
			_attacker.GlobalPosition.Y
		);

	}

	private bool IsNear()
	{
		return Math.Abs(_warningSprite.GlobalPosition.Y - _plr.GlobalPosition.Y) <= _distance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsNear())
		{
			_isInAttackRange = true;
		}

		if (_isInAttackRange)
		{
			_tick += delta;
			_warningToggleTick += delta;
		}

		var attacking = _tick >= _warningTime;

		if (_warningToggleTick >= _warningToggle && !attacking)
		{
			_warningSprite.Visible = !_warningSprite.Visible;
			_warningToggleTick = 0;
			_warningToggle -= 0.03f;
		}
		
		if (attacking)
		{
			_warningSprite.Visible = false;
			Attack(delta);
		}

		if (_tick >= _warningTime * 3)
		{
			// Kills self
			QueueFree();
		}
	}

	private void Attack(double delta)
	{
		var velocity = new Vector2((_attacker.Velocity.X == 0 ? 1 : _attacker.Velocity.X) * (float) delta * _speed, 0);
		if (Math.Abs(velocity.X) >= _topSpeed) velocity = new Vector2(!IsGoingRight ? -_topSpeed : _topSpeed, 0);
		if (!IsGoingRight && velocity.X >= 0) velocity *= -1; 
		_attacker.Velocity = velocity;
		_attacker.MoveAndSlide();
	}
}
