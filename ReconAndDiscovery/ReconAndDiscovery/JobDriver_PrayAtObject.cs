using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobDriver_PrayAtObject : JobDriver
	{
		public JobDriver_PrayAtObject()
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
			Toil t2 = Toils_General.Wait(6000);
			t2.AddFailCondition(() => this.Building.GetComp<CompWeatherSat>() == null);
			t2 = t2.WithEffect(EffecterDefOf.Research, TargetIndex.A);
			t2.tickAction = delegate()
			{
				float num = 0.0002f;
				num *= 1f + 0.5f * (float)base.GetActor().story.traits.DegreeOfTrait(TraitDef.Named("PsychicSensitivity"));
				CompWeatherSat comp = this.Building.GetComp<CompWeatherSat>();
				if (comp != null)
				{
					comp.mana += num;
					if (comp.mana < 0f)
					{
						comp.mana = 0f;
					}
					if (comp.mana > 100f)
					{
						comp.mana = 100f;
					}
				}
			};
			yield return t2;
			yield return Toils_Reserve.Release(TargetIndex.A);
			yield break;
		}

		private const TargetIndex GateIndex = TargetIndex.A;
	}
}
