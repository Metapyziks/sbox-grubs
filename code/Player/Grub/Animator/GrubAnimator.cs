﻿namespace Grubs;

public class GrubAnimator : EntityComponent<Grub>
{
	public virtual void Simulate( IClient client )
	{
		var grub = Entity;
		var ctrl = grub.Controller;

		if ( ctrl is null )
			return;

		grub.SetAnimParameter( "grounded", ctrl.IsGrounded );
		grub.SetAnimParameter( "aimangle", grub.EyeRotation.Pitch() * -grub.Facing );
		grub.SetAnimParameter( "velocity", ctrl.GetWishVelocity().Length );
		grub.SetAnimParameter( "lowhp", grub.Health <= 20f );
		grub.SetAnimParameter( "explode", grub.LifeState == LifeState.Dying );

		var airMove = ctrl.GetMechanic<AirMoveMechanic>();
		if ( airMove is not null )
			grub.SetAnimParameter( "hardfall", airMove.IsHardFalling );

		var holdPose = HoldPose.None;
		if ( grub.IsTurn && grub.ActiveWeapon is not null && ctrl.ShouldShowWeapon() )
			holdPose = grub.ActiveWeapon.HoldPose;
		grub.SetAnimParameter( "holdpose", (int)holdPose );
	}
}
