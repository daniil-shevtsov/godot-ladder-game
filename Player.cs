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
		_isGrounded = floorRaycast.IsColliding();

		Vector3 gravityForce = new Vector3(0, -9.8f, 0);
		ApplyCentralImpulse(gravityForce);
		GD.Print($"Current velocity {LinearVelocity} grounded={_isGrounded}");

		Vector3 velocity = _velocity;
		Vector3 impulse = velocity * Mass;

		ApplyCentralImpulse(impulse);
	}

	public void MoveAndSlide()
	{

	}

	public bool IsOnFloor()
	{
		return floorRaycast.IsColliding();
	}
}
