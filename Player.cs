using Godot;
using System;

public partial class Player : CharacterBody3D {
    public const float SPEED = 5.0f;
    public const float JUMP_VELOCITY = 4.5f;

    public float gravity = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");

    public PlayerInput input;

    private int _player;
    [Export]
    public int player {
        get {
            return _player;
        }
        set {
            _player = value;
            input.SetMultiplayerAuthority(value);
        }
    }

    public override void _Ready() {
        input = GetNode<PlayerInput>("PlayerInput");

        if (player == Multiplayer.GetUniqueId()) {
            GetNode<Camera3D>("Camera3D").Current = true;
        }
    }

    public override void _PhysicsProcess(double delta) {

        Vector3 tempVelocity = Velocity;

        if (!IsOnFloor()) {
            tempVelocity.Y -= gravity * (float) delta;
        }

        if (input.jumping && IsOnFloor()) {
            tempVelocity.Y = JUMP_VELOCITY;
        }

        input.jumping = false;

        Vector3 direction = (Transform.Basis * new Vector3(input.direction.X, 0, input.direction.Y)).Normalized();

        if (direction != Vector3.Zero) {
            tempVelocity.X = direction.X * SPEED;
            tempVelocity.Z = direction.Z * SPEED;
        }

        else {
            tempVelocity.X = Mathf.MoveToward(tempVelocity.X, 0, SPEED);
            tempVelocity.Z = Mathf.MoveToward(tempVelocity.Z, 0, SPEED);
        }

        Velocity = tempVelocity;
        MoveAndSlide();
    }

}
