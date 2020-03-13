using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_DiscoverStargates : JobDriver
	{
		public JobDriver_DiscoverStargates()
		{
			this.rotateToFace = TargetIndex.A;
		}

		private Building Stargate
		{
			get
			{
				return (Building)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private void TriggerFindStargate()
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, 1, Find.World);
			incidentParms.forced = true;
			QueuedIncident qi = new QueuedIncident(new FiringIncident(ThingDefOfReconAndDiscovery.DiscoveredStargate, null, incidentParms), Find.TickManager.TicksGame + 100);
			Find.Storyteller.incidentQueue.Add(qi);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil t2 = Toils_General.Wait(1000);
			t2 = t2.WithEffect(EffecterDefOf.Research, TargetIndex.A);
			float fResearchScore = base.GetActor().GetStatValue(StatDefOf.ResearchSpeed, true);
			t2.tickAction = delegate()
			{
				if (180000f * Rand.Value < fResearchScore)
				{
					this.TriggerFindStargate();
					this.EndJobWith(JobCondition.Succeeded);
				}
			};
			yield return t2;
			yield return Toils_Reserve.Release(TargetIndex.A);
			yield break;
		}

		private const TargetIndex GateIndex = TargetIndex.A;

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2U:
					t2 = Toils_General.Wait(1000);
					t2 = t2.WithEffect(EffecterDefOf.Research, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.fResearchScore = base.GetActor().GetStatValue(StatDefOf.ResearchSpeed, true);
					t2.tickAction = delegate()
					{
						if (180000f * Rand.Value < <MakeNewToils>c__AnonStorey.fResearchScore)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TriggerFindStargate();
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.EndJobWith(JobCondition.Succeeded);
						}
					};
					this.$current = t2;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3U:
					this.$current = Toils_Reserve.Release(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4U:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_DiscoverStargates.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_DiscoverStargates.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal Toil <t2>__0;

			internal JobDriver_DiscoverStargates $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_DiscoverStargates.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					if (180000f * Rand.Value < this.fResearchScore)
					{
						this.<>f__ref$0.$this.TriggerFindStargate();
						this.<>f__ref$0.$this.EndJobWith(JobCondition.Succeeded);
					}
				}

				internal float fResearchScore;

				internal JobDriver_DiscoverStargates.<MakeNewToils>c__Iterator0 <>f__ref$0;
			}
		}
	}
}
