using Godot;
using System;

public partial class Game : Node3D
{
	private Player player;
	private Marker3D cameraPivot;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		cameraPivot = GetNode<Marker3D>("CameraPivot");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cameraPivot.GlobalPosition = player.GlobalPosition;
	}
}
