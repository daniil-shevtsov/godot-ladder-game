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

	private bool isClimbing = false;

	private float projectGravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Player)FindChild("Player");
		hand = (StaticBody3D)FindChild("Hand");
		ladderBottomHinge = (HingeJoint3D)FindChild("LadderBottomHinge");
		ladder = GetNode<Ladder>("Ladder");
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

		HandlePlayerInputs((float)delta);

		if (Input.IsActionJustPressed("toggle_mouse"))
		{
			if (Input.MouseMode == Input.MouseModeEnum.Visible)
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}

		}

		//debugDraw.UpdateVectorToDraw("player velocity", player.GlobalPosition, player.GlobalPosition + player.Velocity);
	}

	private void HandlePlayerInputs(float delta)
	{
		if (!isClimbing)
		{
			Vector3 velocity = player.Velocity;

			if (!player.IsOnFloor())
			{
				velocity.Y -= player.gravity * delta;
			}

			if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
			{
				velocity.Y = Player.JumpVelocity;

			}

			Vector2 inputDir = Input.GetVector("player_left", "player_right", "player_forward", "player_backwards");

			Vector3 direction = (player.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * Player.Speed;
				velocity.Z = direction.Z * Player.Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(player.Velocity.X, 0, Player.Speed);
				velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, Player.Speed);
			}

			player.Velocity = velocity;
			player.MoveAndSlide();
		}
		else
		{
			player.Velocity = Vector3.Zero;

			var climbInput = Input.GetAxis("player_backwards", "player_forward");
			if (climbInput != 0f)
			{
				var multiplier = climbInput * Player.Speed;
				// debugDraw.UpdateVectorToDraw("ladder direction", ladder.GlobalPosition, ladderEnd.GlobalPosition);
				var a = ladderEnd.GlobalPosition - ladder.GlobalPosition;
				var b = a.Normalized();
				debugDraw.UpdateVectorToDraw("a", ladder.GlobalPosition, ladder.GlobalPosition + a * multiplier);
				debugDraw.UpdateVectorToDraw("b", ladder.GlobalPosition, ladder.GlobalPosition + b * multiplier, new Color(1, 0, 0));

				player.Velocity = b * multiplier;
				// debugDraw.UpdateVectorToDraw("player ladder velocity", player.GlobalPosition, player.GlobalPosition + player.Velocity);
				player.MoveAndSlide();
				//player.GlobalPosition += player.Velocity * delta;
			}
			// There is no way to disable project gravity for CharacterBody3D, so we counteract it instead
			player.Velocity += new Vector3(0f, projectGravity * delta, 0f);


		}

		if (isClimbing)
		{
			var ladderNormal = (ladderTop.GlobalPosition - ladder.GlobalPosition).Normalized();
			debugDraw.UpdateVectorToDraw("ladder normal", ladder.GlobalPosition, ladder.GlobalPosition + ladderNormal * 25f, new Color(0, 1, 1));

			var playerToLadder = ladder.GlobalPosition - player.GlobalPosition;

			var ladderPlane = new Plane(ladder.GlobalPosition, ladderNormal);
			var distance = Mathf.Abs(ladderPlane.DistanceTo(player.GlobalPosition));

			var maxDistance = 4f;
			var isTooFar = distance > maxDistance;

			if (isTooFar)
			{
				GD.Print($"distance {distance}");

				//player.Velocity += playerToLadder * distance;
				var newPosition = player.GlobalPosition + playerToLadder.Normalized() * (Mathf.Abs(distance) - maxDistance);//player.GlobalPosition + player.Velocity;
				debugDraw.UpdateVectorToDraw("stick to ladder", player.GlobalPosition, newPosition, new Color(1, 1, 0));

				// player.GlobalPosition = newPosition;
				//player.MoveAndSlide();
			}
		}

		if (Input.IsActionJustPressed("grab"))
		{
			OnGrabPressed();
		}

		if (Input.IsActionJustPressed("use"))
		{
			OnUsePressed();
		}

		if (Input.IsActionPressed("push_ladder_up"))
		{
			ApplyLadderForce(1);
		}
		else if (Input.IsActionPressed("push_ladder_down"))
		{
			ApplyLadderForce(-1);
		}
	}

	private void ApplyLadderForce(int direction)
	{
		var start = ladder.GlobalPosition;
		var end = ladderTop.GlobalPosition;
		var topDirection = start.DirectionTo(end);
		// debugDraw.UpdateVectorToDraw("ladder top", start, end);

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
			ladder.Rotation = player.Rotation;//ladderInitialRotation;

			ladderBottomHinge.NodeA = grabbedPath;
			ladderBottomHinge.NodeB = ladderBottomHinge.GetPathTo(ladder);
		}
		GD.Print($"New nodeA: {ladderBottomHinge.NodeA}");
	}

	private void OnUsePressed()
	{
		isClimbing = !isClimbing;
	}

	private float sensitivity = 1000f;
	private float ladderPushForce = 10f;
}
