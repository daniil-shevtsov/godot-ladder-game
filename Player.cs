using Godot;
using System;

public partial class Player : RigidBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public float MaxSlopeAngle = 45.0f;
	public float Gravity = -9.8f;

	private bool _isGrounded = false;
	private Vector3 _velocity = Vector3.Zero;
	public Vector3 Velocity
	{
		get => _velocity;
		set => _velocity = value;
	}


	public CollisionShape3D CollisionShape;
	public CapsuleShape3D Shape;
	public RayCast3D floorRaycast;

	public override void _Ready()
	{
		base._Ready();
		CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
		Shape = (CapsuleShape3D)CollisionShape.Shape;
		floorRaycast = GetNode<RayCast3D>("FloorRaycast");
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{

	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		// Reset isGrounded to false at the start of each frame
		_isGrounded = false;

		// Check for ground contact using collision points
		for (int i = 0; i < state.GetContactCount(); i++)
		{
			Vector3 contactNormal = state.GetContactLocalNormal(i);
			if (contactNormal.Dot(Vector3.Up) > Math.Cos(Mathf.DegToRad(MaxSlopeAngle)))
			{
				_isGrounded = true;
				break;
			}
		}

		// // Apply gravity
		// if (!_isGrounded)
		// {
		// 	_velocity.Y += Gravity * state.Step;
		// }
		// else
		// {
		// 	_velocity.Y = 0;
		// }

		// // Apply the velocity to the RigidBody
		// state.LinearVelocity = _velocity;
	}

	public void MoveAndSlide()
	{

	}

	public bool IsOnFloor()
	{
		return floorRaycast.IsColliding();
	}
}
