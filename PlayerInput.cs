using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{

	[Export]
	public bool jumping = false;
	[Export]
	public Vector2 direction = Vector2.Zero;

	public override void _EnterTree() {
		SetMultiplayerAuthority(Multiplayer.GetUniqueId());
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GD.Print("GetMultiplayerAuthority: ", GetMultiplayerAuthority());
		GD.Print("Multiplayer.GetUniqueId(): ", Multiplayer.GetUniqueId());


		SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsActionJustPressed("ui_accept")) {
			Rpc(MethodName.Jump);
		}
	}

	[Rpc] // this may need further specification
	public void Jump() {
		jumping = true;
	}

	// [Rpc(MultiplayerApi.RpcMode.Authority)]
	// public void RemoteSetMultiplayerAuthority(int id) {
	// 	SetMultiplayerAuthority(id);
	// 	SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
	// }
}
