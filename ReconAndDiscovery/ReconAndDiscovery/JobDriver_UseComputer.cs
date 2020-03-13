using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_UseComputer : JobDriver
	{
		public override string GetReport()
		{
			return "Using computer";
		}

		public Building Computer
		{
			get
			{
				return (Building)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					this.Computer.GetComp<CompComputerTerminal>().actionDef.ActivatedAction.TryAction(base.GetActor(), base.GetActor().Map, null);
				}
			};
			yield break;
		}
	}
}
