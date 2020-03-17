﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompPsionicEmanator : ThingComp
	{
		public void DoPsychicShockwave()
		{
			IEnumerable<Pawn> enumerable = from pawn in this.parent.Map.mapPawns.AllPawnsSpawned
			where pawn.HostileTo(this.parent.Faction)
			select pawn;
			foreach (Pawn pawn2 in enumerable)
			{
				if (pawn2.story.traits.HasTrait(TraitDef.Named("PsychicSensitivity")))
				{
					if (pawn2.story.traits.HasTrait(TraitDef.Named("PsychicSensitivity")))
					{
						if (!pawn2.health.hediffSet.HasHediff(HediffDefOf.PsychicShock))
						{
							pawn2.health.AddHediff(HediffDefOf.PsychicShock, null, null);
						}
					}
					else if (!pawn2.health.hediffSet.HasHediff(HediffDef.Named("PsychicAttack")))
					{
						pawn2.health.AddHediff(HediffDef.Named("PsychicAttack"), null, null);
					}
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			return "Awaiting prayers and sacrifices.";
		}

		public void DoBattlePrayer()
		{
			IEnumerable<Pawn> freeColonistsSpawned = this.parent.Map.mapPawns.FreeColonistsSpawned;
			foreach (Pawn pawn in freeColonistsSpawned)
			{
				if (pawn.story.traits.HasTrait(TraitDef.Named("PsychicSensitivity")))
				{
					if (pawn.health.hediffSet.HasHediff(HediffDef.Named("BattlePrayer")))
					{
						Hediff other = HediffMaker.MakeHediff(HediffDef.Named("BattlePrayer"), pawn, null);
						pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("BattlePrayer"), false).TryGetComp<HediffComp_Disappears>().CompPostMerged(other);
					}
					else
					{
						pawn.health.AddHediff(HediffDef.Named("BattlePrayer"), null, null);
					}
				}
			}
		}
	}
}







