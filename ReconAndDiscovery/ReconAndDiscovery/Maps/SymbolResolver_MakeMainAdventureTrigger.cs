using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Triggers;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SymbolResolver_MakeMainAdventureTrigger : SymbolResolver
	{
		public SymbolResolver_MakeMainAdventureTrigger()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp);
		}

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ActivatedActionDef actionDef;
			if (rp.TryGetCustom<ActivatedActionDef>("mainAdventureAction", out actionDef))
			{
				ActionTrigger actionTrigger = null;
				IEnumerable<Thing> source = from t in map.listerThings.AllThings
				where t is ActionTrigger
				select t;
				if (source.Count<Thing>() == 0)
				{
					List<Room> allRooms = map.regionGrid.allRooms;
					if (allRooms.Count == 0)
					{
						Log.Error("Could not find contained room for adventure trigger!");
					}
					else
					{
						Room room = allRooms.RandomElementByWeight((Room r) => 1f / r.GetStat(RoomStatDefOf.Space));
						actionTrigger = new ActionTrigger();
						foreach (IntVec3 item in room.Cells)
						{
							actionTrigger.Cells.Add(item);
						}
						IntVec3 intVec = actionTrigger.Cells.RandomElement<IntVec3>();
						GenSpawn.Spawn(actionTrigger, intVec, map);
					}
				}
				else
				{
					actionTrigger = (source.RandomElement<Thing>() as ActionTrigger);
				}
				if (actionTrigger != null)
				{
					actionTrigger.actionDef = actionDef;
				}
			}
		}

		[CompilerGenerated]
		private static bool <Resolve>m__0(Thing t)
		{
			return t is ActionTrigger;
		}

		[CompilerGenerated]
		private static float <Resolve>m__1(Room r)
		{
			return 1f / r.GetStat(RoomStatDefOf.Space);
		}

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Room, float> <>f__am$cache1;
	}
}
