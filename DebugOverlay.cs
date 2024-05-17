using Godot;
using System;

public partial class DebugOverlay : Control
{

	public Camera3D camera;
	public Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Draw()
	{
		var color = new Color(0, 1, 0);

		var start = camera.UnprojectPosition(player.GlobalPosition);
		var end = camera.UnprojectPosition(player.GlobalPosition + player.Velocity);
		DrawLine(start, end, color, 4.0f);
	}
}
