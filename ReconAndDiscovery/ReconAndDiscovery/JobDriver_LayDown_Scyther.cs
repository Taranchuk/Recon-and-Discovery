using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_LayDown_Scyther : JobDriver_LayDown
	{
		public JobDriver_LayDown_Scyther()
		{
		}

		public override string GetReport()
		{
			string result;
			if (this.asleep)
			{
				result = Translator.Translate("ReportSleeping");
			}
			else if (base.GetActor().RaceProps.IsMechanoid)
			{
				result = "Entering Repair Cycle";
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			IEnumerable<Toil> result;
			if (base.GetActor().RaceProps.IsMechanoid)
			{
				result = this.MechToils();
			}
			else
			{
				result = base.MakeNewToils();
			}
			return result;
		}

		private IEnumerable<Toil> MechToils()
		{
			Toil toil = Toils_General.Wait(6000);
			toil.socialMode = RandomSocialMode.Off;
			toil.initAction = delegate()
			{
				this.ticksLeftThisToil = 6000;
				Building firstBuilding = base.GetActor().Position.GetFirstBuilding(base.GetActor().Map);
				if (firstBuilding is Building_Bed)
				{
					base.GetActor().jobs.curDriver.layingDown = 2;
				}
			};
			toil.tickAction = delegate()
			{
				if (Rand.Chance(0.0004f))
				{
					IEnumerable<Hediff_Injury> injuriesTendable = base.GetActor().health.hediffSet.GetInjuriesTendable();
					if (injuriesTendable.Count<Hediff_Injury>() > 0)
					{
						Hediff_Injury hediff_Injury = injuriesTendable.RandomElement<Hediff_Injury>();
						hediff_Injury.Heal((float)Rand.RangeInclusive(1, 3));
					}
				}
			};
			toil.AddEndCondition(delegate
			{
				JobCondition result;
				if (base.GetActor().health.hediffSet.GetInjuriesTendable().Count<Hediff_Injury>() == 0)
				{
					result = JobCondition.Succeeded;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			yield return toil;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MechToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			[DebuggerHidden]
			public <MechToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					toil = Toils_General.Wait(6000);
					toil.socialMode = RandomSocialMode.Off;
					toil.initAction = delegate()
					{
						this.ticksLeftThisToil = 6000;
						Building firstBuilding = base.GetActor().Position.GetFirstBuilding(base.GetActor().Map);
						if (firstBuilding is Building_Bed)
						{
							base.GetActor().jobs.curDriver.layingDown = 2;
						}
					};
					toil.tickAction = delegate()
					{
						if (Rand.Chance(0.0004f))
						{
							IEnumerable<Hediff_Injury> injuriesTendable = base.GetActor().health.hediffSet.GetInjuriesTendable();
							if (injuriesTendable.Count<Hediff_Injury>() > 0)
							{
								Hediff_Injury hediff_Injury = injuriesTendable.RandomElement<Hediff_Injury>();
								hediff_Injury.Heal((float)Rand.RangeInclusive(1, 3));
							}
						}
					};
					toil.AddEndCondition(delegate
					{
						JobCondition result;
						if (base.GetActor().health.hediffSet.GetInjuriesTendable().Count<Hediff_Injury>() == 0)
						{
							result = JobCondition.Succeeded;
						}
						else
						{
							result = JobCondition.Ongoing;
						}
						return result;
					});
					this.$current = toil;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
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
				JobDriver_LayDown_Scyther.<MechToils>c__Iterator0 <MechToils>c__Iterator = new JobDriver_LayDown_Scyther.<MechToils>c__Iterator0();
				<MechToils>c__Iterator.$this = this;
				return <MechToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.ticksLeftThisToil = 6000;
				Building firstBuilding = base.GetActor().Position.GetFirstBuilding(base.GetActor().Map);
				if (firstBuilding is Building_Bed)
				{
					base.GetActor().jobs.curDriver.layingDown = 2;
				}
			}

			internal void <>m__1()
			{
				if (Rand.Chance(0.0004f))
				{
					IEnumerable<Hediff_Injury> injuriesTendable = base.GetActor().health.hediffSet.GetInjuriesTendable();
					if (injuriesTendable.Count<Hediff_Injury>() > 0)
					{
						Hediff_Injury hediff_Injury = injuriesTendable.RandomElement<Hediff_Injury>();
						hediff_Injury.Heal((float)Rand.RangeInclusive(1, 3));
					}
				}
			}

			internal JobCondition <>m__2()
			{
				JobCondition result;
				if (base.GetActor().health.hediffSet.GetInjuriesTendable().Count<Hediff_Injury>() == 0)
				{
					result = JobCondition.Succeeded;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			}

			internal Toil <toil>__0;

			internal JobDriver_LayDown_Scyther $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
