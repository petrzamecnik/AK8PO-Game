using Godot;
using System;

public partial class player : CharacterBody2D
{
    private const float Speed = 300.0f;
    private const float JumpVelocity = -400.0f;
    private float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private int MaxJumps = 2;
    private int JumpsRemaining;

    
    public override void _Ready()
    {
        JumpsRemaining = MaxJumps;
    }
    
    
    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        ApplyGravity(ref velocity, delta);
        HandleJump(ref velocity);
        HandleHorizontalMovement(ref velocity);

        Velocity = velocity;
        MoveAndSlide();
    }

    private void ApplyGravity(ref Vector2 velocity, double delta)
    {
        if (!IsOnFloor())
            velocity.Y += Gravity * (float)delta;
        else
            JumpsRemaining = MaxJumps;
    }

    private void HandleJump(ref Vector2 velocity)
    {
        if (!Input.IsActionJustPressed("ui_accept") || JumpsRemaining <= 0) return;
        
        velocity.Y = JumpVelocity;
        JumpsRemaining--;
    }
    private void HandleHorizontalMovement(ref Vector2 velocity)
    {
        var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
    }
}