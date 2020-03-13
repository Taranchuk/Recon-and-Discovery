using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompNegotiator : ThingComp
	{
		public CompNegotiator()
		{
		}

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

		[CompilerGenerated]
		private sealed class <CompFloatMenuOptions>c__AnonStorey0
		{
			public <CompFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Job job = new Job(JobDefOfReconAndDiscovery.Negotiate);
				job.targetA = this.$this.parent;
				job.playerForced = true;
				this.selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}

			internal Pawn selPawn;

			internal CompNegotiator $this;
		}
	}
}
