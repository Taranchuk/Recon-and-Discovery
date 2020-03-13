using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Missions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_PeaceTalksFaction : SitePartWorker
	{
		public SitePartWorker_PeaceTalksFaction()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			MapParent mapParent = Find.World.worldObjects.MapParentAt(map.Tile);
			Faction faction = mapParent.Faction;
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, 3, map);
			incidentParms.points = Mathf.Max(incidentParms.points, 250f);
			incidentParms.points *= 2f;
			PawnGroupKindDef factionBase = PawnGroupKindDefOf.FactionBase;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.tile = map.Tile;
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.points = incidentParms.points;
			pawnGroupMakerParms.inhabitants = true;
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(factionBase, pawnGroupMakerParms, true))
			{
				IntVec3 intVec;
				CellFinder.TryFindRandomCellInsideWith(new CellRect(40, 40, map.Size.x - 80, map.Size.z - 80), (IntVec3 c) => c.Standable(map), out intVec);
				GenSpawn.Spawn(pawn, intVec, map);
				list.Add(pawn);
			}
			IntVec3 intVec2;
			CellFinder.TryFindRandomCellInsideWith(new CellRect(50, 50, map.Size.x - 100, map.Size.z - 100), (IntVec3 c) => c.Standable(map), out intVec2);
			if (faction.leader != null)
			{
				GenSpawn.Spawn(faction.leader, intVec2, map);
				mapParent.GetComponent<QuestComp_PeaceTalks>().Negotiator = faction.leader;
				list.Add(faction.leader);
			}
			LordJob lordJob = new LordJob_DefendBase(faction, intVec2);
			LordMaker.MakeNewLord(faction, lordJob, map, list);
		}

		public FloatRange casualtiesRange = new FloatRange(400f, 1000f);

		[CompilerGenerated]
		private sealed class <PostMapGenerate>c__AnonStorey0
		{
			public <PostMapGenerate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return c.Standable(this.map);
			}

			internal bool <>m__1(IntVec3 c)
			{
				return c.Standable(this.map);
			}

			internal Map map;
		}
	}
}
