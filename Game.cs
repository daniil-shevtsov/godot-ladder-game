using Godot;
using System;

public partial class Game : Node3D
{
	private Player player;
	private StaticBody3D hand;
	private HingeJoint3D ladderBottomHinge;
	private Ladder ladder;
	private CollisionShape3D ladderEnd;
	private Marker3D ladderTop;
	private Marker3D cameraPivot;
	private Camera3D camera;
	private CanvasLayer debugOverlay;
	private DebugOverlay debugDraw;

	private Vector3 ladderInitialRotation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Input.MouseMode = Input.MouseModeEnum.Captured;
		player = (Player)FindChild("Player");
		hand = (StaticBody3D)FindChild("Hand");
		ladderBottomHinge = (HingeJoint3D)FindChild("LadderBottomHinge");
		ladder = GetNode<Ladder>("Ladder");
		ladderInitialRotation = ladderBottomHinge.Rotation;
		ladderEnd = (CollisionShape3D)FindChild("LadderEnd");
		ladderTop = (Marker3D)FindChild("LadderTop");
		cameraPivot = (Marker3D)FindChild("CameraPivot");
		camera = (Camera3D)FindChild("Camera3D");

		debugOverlay = GetNode<CanvasLayer>("DebugOverlay");
		debugDraw = (DebugOverlay)FindChild("DebugDraw3D");
		debugDraw.camera = camera;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cameraPivot.GlobalPosition = player.GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			GetTree().Quit();
		}

		if (Input.IsActionJustPressed("grab"))
		{
			OnGrabPressed();
		}

		if (Input.IsActionPressed("push_ladder_up"))
		{
			ApplyLadderForce(1);
		}
		else if (Input.IsActionPressed("push_ladder_down"))
		{
			ApplyLadderForce(-1);
		}

		//debugDraw.UpdateVectorToDraw("player velocity", player.GlobalPosition, player.GlobalPosition + player.Velocity);
	}

	private void ApplyLadderForce(int direction)
	{
		var start = ladder.GlobalPosition;
		var end = ladderTop.GlobalPosition;
		var topDirection = start.DirectionTo(end);
		debugDraw.UpdateVectorToDraw("ladder top", start, end);

		ladder.ApplyForce(topDirection * ladderPushForce * direction, ladderEnd.GlobalPosition);
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

	private void OnGrabPressed()
	{
		GD.Print($"Grab pressed, current NodeA: {ladderBottomHinge.NodeA}");
		var grabbedPath = ladderBottomHinge.GetPathTo(hand);
		GD.Print($"Path to: {grabbedPath}");
		if (ladderBottomHinge.NodeA == grabbedPath)
		{
			GD.Print($"Drop");
			ladderBottomHinge.NodeA = null;
			ladderBottomHinge.NodeB = null;
			ladder.SetPhysicsProcess(true);
		}
		else
		{
			GD.Print($"Take");
			ladder.GlobalPosition = new Vector3(
				hand.GlobalPosition.X,
				hand.GlobalPosition.Y + ladder.shape.Size.Y / 2f,
				hand.GlobalPosition.Z
			);
			ladder.Rotation = ladderInitialRotation;

			ladderBottomHinge.NodeA = grabbedPath;
			ladderBottomHinge.NodeB = ladderBottomHinge.GetPathTo(ladder);
		}
		GD.Print($"New nodeA: {ladderBottomHinge.NodeA}");
	}

	private float sensitivity = 1000f;
	private float ladderPushForce = 10f;
}
