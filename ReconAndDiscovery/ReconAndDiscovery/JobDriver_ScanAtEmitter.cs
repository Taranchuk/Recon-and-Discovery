using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ReconAndDiscovery.Things;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_ScanAtEmitter : JobDriver
	{
		public JobDriver_ScanAtEmitter()
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

		private HoloEmitter Emitter
		{
			get
			{
				return (HoloEmitter)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			Toil t2 = Toils_General.Wait(1000);
			t2.AddFailCondition(() => !this.Emitter.GetComp<CompPowerTrader>().PowerOn);
			t2 = t2.WithProgressBar(TargetIndex.A, () => (1000f - (float)this.ticksLeftThisToil) / 1000f, false, -0.5f);
			yield return t2;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					this.Emitter.GetComp<CompHoloEmitter>().Scan(this.Corpse);
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield break;
		}

		private const TargetIndex CorpseIndex = TargetIndex.A;

		private const TargetIndex GraveIndex = TargetIndex.B;

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
					this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2U:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3U:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4U:
					this.$current = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5U:
					t2 = Toils_General.Wait(1000);
					t2.AddFailCondition(() => !base.Emitter.GetComp<CompPowerTrader>().PowerOn);
					t2 = t2.WithProgressBar(TargetIndex.A, () => (1000f - (float)this.ticksLeftThisToil) / 1000f, false, -0.5f);
					this.$current = t2;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6U:
				{
					Toil tScan = new Toil();
					tScan.defaultCompleteMode = ToilCompleteMode.Instant;
					tScan.initAction = delegate()
					{
						base.Emitter.GetComp<CompHoloEmitter>().Scan(base.Corpse);
					};
					this.$current = tScan;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				case 7U:
					this.$current = Toils_Reserve.Release(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8U:
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
				JobDriver_ScanAtEmitter.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ScanAtEmitter.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Emitter.GetComp<CompPowerTrader>().PowerOn;
			}

			internal float <>m__1()
			{
				return (1000f - (float)this.ticksLeftThisToil) / 1000f;
			}

			internal void <>m__2()
			{
				base.Emitter.GetComp<CompHoloEmitter>().Scan(base.Corpse);
			}

			internal Toil <t2>__0;

			internal Toil <tScan>__1;

			internal JobDriver_ScanAtEmitter $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
