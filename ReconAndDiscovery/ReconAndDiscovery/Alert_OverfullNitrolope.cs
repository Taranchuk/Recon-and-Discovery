using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	public class Alert_OverfullNitrolope : Alert
	{
		public Alert_OverfullNitrolope()
		{
			this.defaultLabel = "Explosive risk!";
			this.defaultPriority = AlertPriority.High;
		}

		public override string GetExplanation()
		{
			string result;
			if (this.OverfullNitralope() == null)
			{
				result = string.Empty;
			}
			else
			{
				result = "A nitralope has become dangerously bloated. You must relieve the pressure by milking it, or it may explode!";
			}
			return result;
		}

		public override AlertReport GetReport()
		{
			return this.OverfullNitralope() != null;
		}

		private Pawn OverfullNitralope()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
				{
					if (pawn.Faction == Faction.OfPlayer && pawn.kindDef == ThingDefOfReconAndDiscovery.Nitralope)
					{
						if (pawn.GetComp<CompMandatoryMilkable>().Overfull)
						{
							return pawn;
						}
					}
				}
			}
			return null;
		}
	}
}
