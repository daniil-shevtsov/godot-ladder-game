using Godot;
using System;

public partial class Player : RigidBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public Vector3 Velocity = Vector3.Zero;

	public CollisionShape3D CollisionShape;
	public CapsuleShape3D Shape;

	public override void _Ready()
	{
		base._Ready();
		CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
		Shape = (CapsuleShape3D)CollisionShape.Shape;
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{

	}

	public void MoveAndSlide()
	{

	}

	public bool IsOnFloor()
	{
		return false;
	}
}
