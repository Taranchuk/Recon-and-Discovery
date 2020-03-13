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
	public class JobDriver_TravelThroughStargate : JobDriver
	{
		public JobDriver_TravelThroughStargate()
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

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					CompStargate comp = this.Stargate.GetComp<CompStargate>();
					comp.SendPawnThroughStargate(base.GetActor());
				}
			};
			yield break;
		}

		private const TargetIndex StargateIndex = TargetIndex.A;

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
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
				{
					Toil travel = new Toil();
					travel.defaultCompleteMode = ToilCompleteMode.Instant;
					travel.initAction = delegate()
					{
						CompStargate comp = base.Stargate.GetComp<CompStargate>();
						comp.SendPawnThroughStargate(base.GetActor());
					};
					this.$current = travel;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2U:
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
				JobDriver_TravelThroughStargate.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_TravelThroughStargate.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				CompStargate comp = base.Stargate.GetComp<CompStargate>();
				comp.SendPawnThroughStargate(base.GetActor());
			}

			internal Toil <travel>__0;

			internal JobDriver_TravelThroughStargate $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
