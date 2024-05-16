using Godot;
using System;

[Tool]
public partial class Ground : StaticBody3D
{
	CollisionShape3D collisionShape;
	BoxShape3D boxShape;
	MeshInstance3D meshInstance;
	BoxMesh boxMesh;

	private Vector3 _size = new Vector3(5f, 5f, 5f);
	[Export]
	public Vector3 Size
	{
		get => _size;
		set
		{
			_size = value;
			OnSizeUpdated();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
		boxShape = (BoxShape3D)collisionShape.Shape.Duplicate();
		collisionShape.Shape = boxShape;
		meshInstance = GetNode<MeshInstance3D>("MeshInstance3D");
		boxMesh = (BoxMesh)meshInstance.Mesh.Duplicate();
		meshInstance.Mesh = boxMesh;

		GD.Print($"Init {collisionShape} {boxShape} {meshInstance} {boxMesh}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnSizeUpdated()
	{
		boxShape.Size = _size;
		boxMesh.Size = _size;
		GD.Print($" {boxShape} {boxMesh} {boxShape.Size} {boxMesh.Size} new size: {_size} new collision size: {boxShape.Size}");

	}
}
