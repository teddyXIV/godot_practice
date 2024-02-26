using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 500.0f;
	public const float JumpVelocity = -700.0f;

	private AnimationPlayer anim;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity;

	public override void _Ready()
	{
		// Assign gravity in _Ready instead of directly initializing it.
		gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

		// Find the AnimationPlayer node dynamically
		anim = GetNode<AnimationPlayer>("AnimationPlayer");

		// Ensure the AnimationPlayer node is not null before trying to use it
		if (anim != null)
		{
			anim.Play("Idle");
		}
		else
		{
			GD.PrintErr("AnimationPlayer node not found!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			// Play jump animation only when jumping.
			anim?.Play("Jump");
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			if (direction.X == -1)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
			}
			else
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			}
			velocity.X = direction.X * Speed;
			// Play walk animation only when moving.
			if (velocity.Y == 0)
			{
			anim?.Play("Walk");
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			// Play idle animation when not moving.
			anim?.Play("Idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
