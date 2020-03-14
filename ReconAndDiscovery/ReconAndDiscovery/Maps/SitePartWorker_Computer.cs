﻿using System;
using ReconAndDiscovery.Triggers;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_Computer : SitePartWorker
	{
		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			IntVec3 loc;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && GridsUtility.GetRoom(x, map, 6).CellCount <= 30, map, out loc))
			{
				Thing thing = ThingMaker.MakeThing(ThingDef.Named("QuestComputerTerminal"), null);
				if (this.action != null)
				{
					(thing as Building).GetComp<CompComputerTerminal>().actionDef = this.action;
				}
				GenSpawn.Spawn(thing, loc, map);
			}
		}

		public ActivatedActionDef action;
	}
}