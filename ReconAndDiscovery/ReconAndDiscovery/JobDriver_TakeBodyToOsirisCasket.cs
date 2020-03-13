using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_TakeBodyToOsirisCasket : JobDriver
	{
		public JobDriver_TakeBodyToOsirisCasket()
		{
			this.rotateToFace = TargetIndex.B;
		}

		private Corpse Corpse
		{
			get
			{
				return (Corpse)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building_CryptosleepCasket Casket
		{
			get
			{
				return (Building_CryptosleepCasket)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A);
			yield return Toils_Reserve.Release(TargetIndex.A);
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield break;
		}

		private const TargetIndex CorpseIndex = TargetIndex.A;

		private const TargetIndex GraveIndex = TargetIndex.B;
	}
}
