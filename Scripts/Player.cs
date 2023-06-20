using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D animatedSprite;
	public enum State
	{
		Idle, 
		Walk,
		Jump,
		Attack,
		Hurt,
		Death,
		Fire
	}
	public State currentState = State.Idle;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.AnimationFinished+=OnAnimationFinished;
	}

	private void OnAnimationFinished()
	{
		SetState(State.Idle);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		switch (currentState)
		{
			case State.Idle:
				animatedSprite.Play("Dragon_Idle");
				if (Input.IsActionJustPressed("Jump") && IsOnFloor())
				{
					currentState = State.Jump;
				}
				if (Input.IsActionJustPressed("Left") || Input.IsActionJustPressed("Right"))
				{
					currentState = State.Walk;
				}
				break;
			case State.Walk:
				Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
				if (direction != Vector2.Zero)
				{
					animatedSprite.Play("Dragon_Walk");
					velocity.X = direction.X * Speed;
					if (direction.X < 0)
					{
						animatedSprite.FlipH = true;
					}
					else
					{
						animatedSprite.FlipH = false;
					}
				}
				else
				{
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
					SetState(State.Idle);
				}
				break;
			case State.Jump:
				velocity.Y = JumpVelocity;
				break;
			case State.Attack:
				break;
			case State.Hurt:
				break;
			case State.Death:
				break;
			case State.Fire:
				break;
			default:
				currentState = State.Idle;
				break;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void SetState(State newState)
	{
		currentState = newState;
	}
}
