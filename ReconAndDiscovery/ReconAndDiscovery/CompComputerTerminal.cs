using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Triggers;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class CompComputerTerminal : ThingComp
	{
		public CompComputerTerminal()
		{
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			List<FloatMenuOption> list = base.CompFloatMenuOptions(selPawn).ToList<FloatMenuOption>();
			if (this.actionDef != null && this.parent.GetComp<CompPowerTrader>().PowerOn)
			{
				list.Add(new FloatMenuOption(MenuOptionPriority.Default)
				{
					Label = "Use computer",
					action = delegate()
					{
						selPawn.jobs.TryTakeOrderedJob(this.UseComputerJob(), JobTag.Misc);
					}
				});
			}
			return list;
		}

		public Job UseComputerJob()
		{
			return new Job(JobDefOfReconAndDiscovery.UseComputer, this.parent);
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ActivatedActionDef>(ref this.actionDef, "actionDef");
		}

		public ActivatedActionDef actionDef;

		[CompilerGenerated]
		private sealed class <CompFloatMenuOptions>c__AnonStorey0
		{
			public <CompFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.selPawn.jobs.TryTakeOrderedJob(this.$this.UseComputerJob(), JobTag.Misc);
			}

			internal Pawn selPawn;

			internal CompComputerTerminal $this;
		}
	}
}
