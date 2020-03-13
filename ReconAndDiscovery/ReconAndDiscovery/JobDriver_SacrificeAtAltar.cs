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
	public class JobDriver_SacrificeAtAltar : JobDriver
	{
		public JobDriver_SacrificeAtAltar()
		{
			this.rotateToFace = TargetIndex.B;
		}

		private Pawn Animal
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building Altar
		{
			get
			{
				return (Building)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, 1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return Toils_General.Wait(1500).WithProgressBar(TargetIndex.B, () => 1f - 1f * (float)this.ticksLeftThisToil / 1500f, false, -0.5f);
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					DamageInfo value = new DamageInfo(DamageDefOf.ExecutionCut, 500, -1f, base.GetActor(), null, null, DamageInfo.SourceCategory.ThingOrUnknown);
					this.Animal.Kill(new DamageInfo?(value));
					this.Altar.GetComp<CompPsionicEmanator>().DoPsychicShockwave();
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield break;
		}

		private const TargetIndex AnimalIndex = TargetIndex.A;

		private const TargetIndex AltarIndex = TargetIndex.B;

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
					this.$current = Toils_Reserve.Reserve(TargetIndex.A, 1, 1, null);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, 1, null);
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
					this.$current = Toils_General.Wait(1500).WithProgressBar(TargetIndex.B, () => 1f - 1f * (float)this.ticksLeftThisToil / 1500f, false, -0.5f);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6U:
				{
					Toil sacrificeToil = new Toil();
					sacrificeToil.defaultCompleteMode = ToilCompleteMode.Instant;
					sacrificeToil.initAction = delegate()
					{
						DamageInfo value = new DamageInfo(DamageDefOf.ExecutionCut, 500, -1f, base.GetActor(), null, null, DamageInfo.SourceCategory.ThingOrUnknown);
						base.Animal.Kill(new DamageInfo?(value));
						base.Altar.GetComp<CompPsionicEmanator>().DoPsychicShockwave();
					};
					this.$current = sacrificeToil;
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
				JobDriver_SacrificeAtAltar.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_SacrificeAtAltar.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal float <>m__0()
			{
				return 1f - 1f * (float)this.ticksLeftThisToil / 1500f;
			}

			internal void <>m__1()
			{
				DamageInfo value = new DamageInfo(DamageDefOf.ExecutionCut, 500, -1f, base.GetActor(), null, null, DamageInfo.SourceCategory.ThingOrUnknown);
				base.Animal.Kill(new DamageInfo?(value));
				base.Altar.GetComp<CompPsionicEmanator>().DoPsychicShockwave();
			}

			internal Toil <sacrificeToil>__0;

			internal JobDriver_SacrificeAtAltar $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
