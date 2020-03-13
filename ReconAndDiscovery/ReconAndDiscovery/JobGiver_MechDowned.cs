using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class JobGiver_MechDowned : ThinkNode_JobGiver
	{
		public JobGiver_MechDowned()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.InBed())
			{
				result = new Job(JobDefOf.LayDown);
			}
			else
			{
				result = new Job(JobDefOf.WaitDowned);
			}
			return result;
		}
	}
}
