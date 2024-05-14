using Godot;
using System;

public partial class Game : Node3D
{
	private Player player;
	private Ladder ladder;
	private CollisionShape3D ladderEnd;
	private Marker3D cameraPivot;
	private Camera3D camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Player)FindChild("Player");
		ladder = GetNode<Ladder>("Ladder");
		ladderEnd = (CollisionShape3D)FindChild("LadderEnd");
		cameraPivot = (Marker3D)FindChild("CameraPivot");
		camera = (Camera3D)FindChild("Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cameraPivot.GlobalPosition = player.GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("push_ladder_up"))
		{
			ladder.ApplyForce(new Vector3(0f, ladderPushForce, 0f), ladderEnd.GlobalPosition);
		}
		else if (Input.IsActionJustPressed("push_ladder_down"))
		{
			ladder.ApplyForce(new Vector3(0f, -ladderPushForce, 0f), ladderEnd.GlobalPosition);
		}
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
			cameraPivot.Rotation = new Vector3(
				Mathf.Clamp(cameraPivot.Rotation.X - eventMouseMotion.Relative.Y / sensitivity, Mathf.DegToRad(-45f), Mathf.DegToRad(90f)),
				cameraPivot.Rotation.Y,
				cameraPivot.Rotation.Z
			);
		}
	}

	private float sensitivity = 1000f;
	private float ladderPushForce = 100f;
}
