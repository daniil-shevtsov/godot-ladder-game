using Godot;
using System;
using System.Drawing;

public partial class Ladder : RigidBody3D
{

	public Area3D shapeArea;
	private CollisionShape3D collisionShape;
	private BoxShape3D shape;
	public Vector3 Size
	{
		get
		{
			return shape.Size;
		}
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shapeArea = GetNode<Area3D>("ShapeArea");
		collisionShape = shapeArea.GetNode<CollisionShape3D>("CollisionShape3D");
		shape = (BoxShape3D)collisionShape.Shape;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
