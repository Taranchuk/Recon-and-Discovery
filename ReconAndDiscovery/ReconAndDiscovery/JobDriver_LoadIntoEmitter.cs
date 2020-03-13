using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ReconAndDiscovery.Things;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_LoadIntoEmitter : JobDriver
	{
		public JobDriver_LoadIntoEmitter()
		{
			this.rotateToFace = TargetIndex.B;
		}

		private Pawn MakeGeniusPawn()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, Faction.OfPlayer, PawnGenerationContext.PlayerStarter, -1, true, false, false, false, false, false, 0f, true, true, true, false, false, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			List<Trait> list = new List<Trait>();
			foreach (Trait trait in pawn.story.traits.allTraits)
			{
				if (trait.def == TraitDefOf.Psychopath || trait.def == TraitDefOf.Cannibal || trait.def == TraitDefOf.Pyromaniac || trait.def == TraitDefOf.Prosthophobe)
				{
					list.Add(trait);
				}
			}
			foreach (Trait item in list)
			{
				pawn.story.traits.allTraits.Remove(item);
			}
			List<SkillRecord> list2 = (from s in pawn.skills.skills
			where !s.TotallyDisabled
			select s).ToList<SkillRecord>();
			SkillRecord skillRecord = list2.RandomElement<SkillRecord>();
			skillRecord.Level = 20;
			skillRecord.passion = Passion.Major;
			list2.Remove(skillRecord);
			skillRecord = list2.RandomElement<SkillRecord>();
			skillRecord.Level = 20;
			skillRecord.passion = Passion.Major;
			return pawn;
		}

		private Thing Disk
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
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
					Pawn simPawn = this.MakeGeniusPawn();
					this.Emitter.GetComp<CompHoloEmitter>().SimPawn = simPawn;
					this.Emitter.GetComp<CompHoloEmitter>().SetUpPawn();
					this.Disk.Destroy(DestroyMode.Vanish);
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield break;
		}

		[CompilerGenerated]
		private static bool <MakeGeniusPawn>m__0(SkillRecord s)
		{
			return !s.TotallyDisabled;
		}

		private const TargetIndex CorpseIndex = TargetIndex.A;

		private const TargetIndex GraveIndex = TargetIndex.B;

		[CompilerGenerated]
		private static Func<SkillRecord, bool> <>f__am$cache0;

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
						Pawn simPawn = base.MakeGeniusPawn();
						base.Emitter.GetComp<CompHoloEmitter>().SimPawn = simPawn;
						base.Emitter.GetComp<CompHoloEmitter>().SetUpPawn();
						base.Disk.Destroy(DestroyMode.Vanish);
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
				JobDriver_LoadIntoEmitter.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_LoadIntoEmitter.<MakeNewToils>c__Iterator0();
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
				Pawn simPawn = base.MakeGeniusPawn();
				base.Emitter.GetComp<CompHoloEmitter>().SimPawn = simPawn;
				base.Emitter.GetComp<CompHoloEmitter>().SetUpPawn();
				base.Disk.Destroy(DestroyMode.Vanish);
			}

			internal Toil <t2>__0;

			internal Toil <tScan>__1;

			internal JobDriver_LoadIntoEmitter $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;
		}
	}
}
