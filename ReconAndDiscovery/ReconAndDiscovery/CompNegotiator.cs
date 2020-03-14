using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompNegotiator : ThingComp
	{
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			List<FloatMenuOption> list = base.CompFloatMenuOptions(selPawn).ToList<FloatMenuOption>();
			FloatMenuOption item = new FloatMenuOption("Negotiate", delegate()
			{
				Job job = new Job(JobDefOfReconAndDiscovery.Negotiate);
				job.targetA = this.parent;
				job.playerForced = true;
				selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			list.Add(item);
			return list;
		}

		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 200 == 1)
			{
				MoteMaker.ThrowMetaIcon(this.parent.Position, this.parent.Map, ThingDef.Named("Mote_Laurel"));
			}
		}
	}
}
