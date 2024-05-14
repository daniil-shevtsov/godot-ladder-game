using Godot;
using System;

public partial class Game : Node3D
{
	private Player player;
	private Ladder ladder;
	private Marker3D cameraPivot;
	private Camera3D camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		ladder = GetNode<Ladder>("Ladder");
		cameraPivot = GetNode<Marker3D>("CameraPivot");
		camera = (Camera3D)FindChild("Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cameraPivot.GlobalPosition = player.GlobalPosition;
	}

	// accumulators
	private float _rotationX = 0f;
	private float _rotationY = 0f;
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			player.Rotation = new Vector3(
				player.Rotation.X,
				player.Rotation.Y - eventMouseMotion.Relative.X / sensitivity,
				player.Rotation.Z
			);
		}
	}

	private float sensitivity = 1000f;
}
