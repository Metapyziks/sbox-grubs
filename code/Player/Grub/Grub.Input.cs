﻿namespace Grubs;

public partial class Grub
{
	public float LookInput { get; set; }

	[Net, Predicted]
	public float MoveInput { get; set; }

	[Net, Predicted]
	public Angles LookAngles { get; set; }

	[HideInEditor]
	public Vector3 EyePosition
	{
		get => Transform.PointToWorld( EyeLocalPosition );
		set => EyeLocalPosition = Transform.PointToLocal( value );
	}

	[Net, Predicted, HideInEditor]
	public Vector3 EyeLocalPosition { get; set; }

	[HideInEditor]
	public Rotation EyeRotation
	{
		get => Transform.RotationToWorld( EyeLocalRotation );
		set => EyeLocalRotation = Transform.RotationToLocal( value );
	}

	[Net, Predicted, HideInEditor]
	public Rotation EyeLocalRotation { get; set; }

	[Net, Predicted]
	public int Facing { get; set; } = 1;

	[Net, Predicted]
	public int LastFacing { get; set; } = 1;

	public override Ray AimRay => new( EyePosition, Facing * EyeRotation.Forward );

	[Net, Predicted]
	public float SnappedLookAngle { get; set; } = 0f;

	[Net, Predicted]
	public bool ChangedSnapAngle { get; set; } = false;

	public void UpdateInputFromOwner( float moveInput, float lookInput )
	{
		var nextFacing = Rotation.z < 0 ? -1 : 1;

		MoveInput = moveInput;
		LookInput = -nextFacing * (lookInput * 1.4f);

		if ( ActiveWeapon.IsValid() && ActiveWeapon.IsCharging() )
			return;

		var look = (LookAngles + LookInput).Normal;
		LookAngles = look
			.WithPitch( look.pitch.Clamp( -80f, 75f ) )
			.WithRoll( 0f )
			.WithYaw( 0f );

		if ( nextFacing != LastFacing )
			LookAngles = LookAngles.WithPitch( LookAngles.pitch * -1 );

		LastFacing = nextFacing;

		if ( Debug && IsTurn )
		{
			DebugOverlay.ScreenText( $"MoveInput: {MoveInput}", 13 );
			DebugOverlay.ScreenText( $"LookInput: {LookInput}", 14 );
			DebugOverlay.ScreenText( $"LookAngles: {LookAngles}", 15 );
			DebugOverlay.ScreenText( $"Facing: {Facing}", 16 );
			DebugOverlay.ScreenText( $"LastFacing: {LastFacing}", 17 );
			var tr = Trace.Ray( AimRay, 80f ).Run();
			DebugOverlay.TraceResult( tr );
		}
	}

	[ConVar.Replicated( "gr_debug_input" )]
	public static bool Debug { get; set; } = false;
}
