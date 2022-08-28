﻿namespace Grubs.Player;

[Category( "Spectators" )]
public class Spectator : Entity, ISpectator
{
	/// <summary>
	/// The camera that the team client will see the game through.
	/// </summary>
	public CameraMode Camera
	{
		get => Components.Get<CameraMode>();
		private set => Components.Add( value );
	}

	public Spectator()
	{
		Camera = new GrubsCamera();
	}
}
