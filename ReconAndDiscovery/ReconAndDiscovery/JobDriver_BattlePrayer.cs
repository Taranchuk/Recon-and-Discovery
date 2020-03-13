using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_BattlePrayer : JobDriver
	{
		public JobDriver_BattlePrayer()
		{
			this.rotateToFace = TargetIndex.A;
		}

		private Building Building
		{
			get
			{
				return (Building)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 4, 0, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil t2 = Toils_General.Wait(10000);
			t2.tickAction = delegate()
			{
				if (this.ticksLeftThisToil % 100 == 50)
				{
					this.Building.GetComp<CompPsionicEmanator>().DoBattlePrayer();
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
					this.$current = Toils_Reserve.Reserve(TargetIndex.A, 4, 0, null);
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
					t2 = Toils_General.Wait(10000);
					t2.tickAction = delegate()
					{
						if (this.ticksLeftThisToil % 100 == 50)
						{
							base.Building.GetComp<CompPsionicEmanator>().DoBattlePrayer();
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
				JobDriver_BattlePrayer.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_BattlePrayer.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				if (this.ticksLeftThisToil % 100 == 50)
				{
					base.Building.GetComp<CompPsionicEmanator>().DoBattlePrayer();
				}
			}

			internal Toil <t2>__0;

			internal JobDriver_BattlePrayer $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
