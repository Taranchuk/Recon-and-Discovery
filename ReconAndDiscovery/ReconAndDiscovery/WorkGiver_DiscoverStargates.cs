﻿using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class WorkGiver_DiscoverStargates : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDef.Named("Stargate"));
			}
		}

		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
		}

		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			return building != null && ReservationUtility.CanReserve(pawn, t, 1, -1, null, forced);
		}

		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOfReconAndDiscovery.DiscoverStargates, t);
		}
	}
}
