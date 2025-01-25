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
	[Export] public float WarningTime = 5;
	private float _warningToggle = 0.5f;
	private double _warningToggleTick;
	private double _tick;
	[Export] public int Distance = 300;
	[Export] public float Speed = 150;
	[Export] public float TopSpeed = 4000;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_warningSprite = GetNode<AnimatedSprite2D>("./AttackWarning");
		_attacker = GetNode<CharacterBody2D>("./AttackPattern");
		_plr = GetNode<CharacterBody2D>("../bob");
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
		GD.Print("Distance: ", Math.Abs(_warningSprite.GlobalPosition.Y - _plr.GlobalPosition.Y));
		return Math.Abs(_warningSprite.GlobalPosition.Y - _plr.GlobalPosition.Y) <= Distance;
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

		var attacking = _tick >= WarningTime;

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

		if (_tick >= WarningTime * 3)
		{
			// Kills self
			QueueFree();
		}
	}

	private void Attack(double delta)
	{
		var velocity = new Vector2((_attacker.Velocity.X == 0 ? 1 : _attacker.Velocity.X) * (float) delta * Speed, 0);
		if (Math.Abs(velocity.X) >= TopSpeed) velocity = new Vector2(!IsGoingRight ? -TopSpeed : TopSpeed, 0);
		if (!IsGoingRight && velocity.X >= 0) velocity *= -1; 
		_attacker.Velocity = velocity;
		_attacker.MoveAndSlide();
	}
}
