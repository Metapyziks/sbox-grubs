﻿namespace Grubs;

public partial class Player
{
	/// <summary>
	/// MoveInput is fed by Input.AnalogMove in the y-axis.
	/// </summary>
	[ClientInput]
	public float MoveInput { get; set; }

	/// <summary>
	/// LookInput is fed by Input.AnalogMove in x-axis.
	/// </summary>
	[ClientInput]
	public float LookInput { get; set; }

	public override void BuildInput()
	{
		if ( Input.StopProcessing )
			return;

		MoveInput = Input.AnalogMove.y;
		LookInput = Input.AnalogMove.x;
	}
}