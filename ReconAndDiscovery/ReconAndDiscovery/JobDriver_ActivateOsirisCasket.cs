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
	public class JobDriver_ActivateOsirisCasket : JobDriver
	{
		public JobDriver_ActivateOsirisCasket()
		{
			this.rotateToFace = TargetIndex.A;
		}

		private OsirisCasket Casket
		{
			get
			{
				return (OsirisCasket)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			Toil tFuel = new Toil();
			tFuel.defaultCompleteMode = ToilCompleteMode.Instant;
			tFuel.AddFailCondition(() => this.Casket.GetComp<CompRefuelable>().Fuel < 50f);
			tFuel.AddFailCondition(() => !this.Casket.GetComp<CompPowerTrader>().PowerOn);
			tFuel.initAction = delegate()
			{
				this.Casket.GetComp<CompRefuelable>().ConsumeFuel(25f);
			};
			yield return tFuel;
			Toil t2 = Toils_General.Wait(6000);
			t2.AddFailCondition(() => !this.Casket.GetComp<CompPowerTrader>().PowerOn);
			t2.initAction = delegate()
			{
				this.Casket.Map.weatherManager.TransitionTo(WeatherDef.Named("RainyThunderstorm"));
			};
			t2 = t2.WithProgressBar(TargetIndex.A, () => (6000f - (float)this.ticksLeftThisToil) / 6000f, false, -0.5f);
			yield return t2;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					GenExplosion.DoExplosion(this.Casket.Position, this.Casket.Map, 50f, DamageDefOf.EMP, this.Casket, SoundDefOf.EnergyShieldBroken, null, null, null, 0f, 1, false, null, 0f, 1);
					this.Casket.GetComp<CompOsiris>().HealContained();
					this.Casket.Map.weatherManager.TransitionTo(WeatherDef.Named("Rain"));
					IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, 3, this.Casket.Map);
					incidentParms.forced = true;
					incidentParms.target = this.Casket.Map;
					QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDef.Named("ShortCircuit"), null, incidentParms), Find.TickManager.TicksGame + 1);
					Find.Storyteller.incidentQueue.Add(qi);
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.A);
			yield break;
		}

		private const TargetIndex CasketIndex = TargetIndex.A;

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
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2U:
					tFuel = new Toil();
					tFuel.defaultCompleteMode = ToilCompleteMode.Instant;
					tFuel.AddFailCondition(() => base.Casket.GetComp<CompRefuelable>().Fuel < 50f);
					tFuel.AddFailCondition(() => !base.Casket.GetComp<CompPowerTrader>().PowerOn);
					tFuel.initAction = delegate()
					{
						base.Casket.GetComp<CompRefuelable>().ConsumeFuel(25f);
					};
					this.$current = tFuel;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3U:
					t2 = Toils_General.Wait(6000);
					t2.AddFailCondition(() => !base.Casket.GetComp<CompPowerTrader>().PowerOn);
					t2.initAction = delegate()
					{
						base.Casket.Map.weatherManager.TransitionTo(WeatherDef.Named("RainyThunderstorm"));
					};
					t2 = t2.WithProgressBar(TargetIndex.A, () => (6000f - (float)this.ticksLeftThisToil) / 6000f, false, -0.5f);
					this.$current = t2;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4U:
				{
					Toil heal = new Toil();
					heal.defaultCompleteMode = ToilCompleteMode.Instant;
					heal.initAction = delegate()
					{
						GenExplosion.DoExplosion(base.Casket.Position, base.Casket.Map, 50f, DamageDefOf.EMP, base.Casket, SoundDefOf.EnergyShieldBroken, null, null, null, 0f, 1, false, null, 0f, 1);
						base.Casket.GetComp<CompOsiris>().HealContained();
						base.Casket.Map.weatherManager.TransitionTo(WeatherDef.Named("Rain"));
						IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, 3, base.Casket.Map);
						incidentParms.forced = true;
						incidentParms.target = base.Casket.Map;
						QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDef.Named("ShortCircuit"), null, incidentParms), Find.TickManager.TicksGame + 1);
						Find.Storyteller.incidentQueue.Add(qi);
					};
					this.$current = heal;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				case 5U:
					this.$current = Toils_Reserve.Release(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6U:
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
				JobDriver_ActivateOsirisCasket.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ActivateOsirisCasket.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return base.Casket.GetComp<CompRefuelable>().Fuel < 50f;
			}

			internal bool <>m__1()
			{
				return !base.Casket.GetComp<CompPowerTrader>().PowerOn;
			}

			internal void <>m__2()
			{
				base.Casket.GetComp<CompRefuelable>().ConsumeFuel(25f);
			}

			internal bool <>m__3()
			{
				return !base.Casket.GetComp<CompPowerTrader>().PowerOn;
			}

			internal void <>m__4()
			{
				base.Casket.Map.weatherManager.TransitionTo(WeatherDef.Named("RainyThunderstorm"));
			}

			internal float <>m__5()
			{
				return (6000f - (float)this.ticksLeftThisToil) / 6000f;
			}

			internal void <>m__6()
			{
				GenExplosion.DoExplosion(base.Casket.Position, base.Casket.Map, 50f, DamageDefOf.EMP, base.Casket, SoundDefOf.EnergyShieldBroken, null, null, null, 0f, 1, false, null, 0f, 1);
				base.Casket.GetComp<CompOsiris>().HealContained();
				base.Casket.Map.weatherManager.TransitionTo(WeatherDef.Named("Rain"));
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, 3, base.Casket.Map);
				incidentParms.forced = true;
				incidentParms.target = base.Casket.Map;
				QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDef.Named("ShortCircuit"), null, incidentParms), Find.TickManager.TicksGame + 1);
				Find.Storyteller.incidentQueue.Add(qi);
			}

			internal Toil <tFuel>__0;

			internal Toil <t2>__1;

			internal Toil <heal>__2;

			internal JobDriver_ActivateOsirisCasket $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
