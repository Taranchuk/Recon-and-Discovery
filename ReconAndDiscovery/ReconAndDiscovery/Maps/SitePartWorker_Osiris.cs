using System;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Triggers;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_Osiris : SitePartWorker
	{
		public SitePartWorker_Osiris()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount <= 30, map, out intVec))
			{
				Thing thing = ThingMaker.MakeThing(ThingDef.Named("OsirisCasket"), null);
				GenSpawn.Spawn(thing, intVec, map);
				OsirisCasket osirisCasket = thing as OsirisCasket;
				Pawn thing2 = PawnGenerator.GeneratePawn(PawnKindDefOf.SpaceSoldier, Find.FactionManager.FirstFactionOfDef(FactionDefOf.SpacerHostile));
				osirisCasket.TryAcceptThing(thing2, true);
			}
		}

		public ActivatedActionDef action;

		[CompilerGenerated]
		private sealed class <PostMapGenerate>c__AnonStorey0
		{
			public <PostMapGenerate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount <= 30;
			}

			internal Map map;
		}
	}
}
