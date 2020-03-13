using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_AdventureGenerator : GenStep
	{
		public GenStep_AdventureGenerator()
		{
		}

		public virtual void Generate(Map map)
		{
			int minX = map.Size.x / 5;
			int width = 3 * map.Size.x / 5;
			int minZ = map.Size.z / 5;
			int height = 3 * map.Size.z / 5;
			this.adventureRegion = new CellRect(minX, minZ, width, height);
			this.adventureRegion.ClipInsideMap(map);
			BaseGen.globalSettings.map = map;
			this.randomRoomEvents.Clear();
			IntVec3 playerStartSpot;
			CellFinder.TryFindRandomEdgeCellWith((IntVec3 v) => v.Standable(map), map, 0f, out playerStartSpot);
			MapGenerator.PlayerStartSpot = playerStartSpot;
			this.baseResolveParams = default(ResolveParams);
			foreach (string text in this.randomRoomEvents.Keys)
			{
				this.baseResolveParams.SetCustom<float>(text, this.randomRoomEvents[text], false);
			}
		}

		protected AdventureWorker adventure = null;

		protected Dictionary<string, float> randomRoomEvents = new Dictionary<string, float>();

		protected CellRect adventureRegion;

		protected ResolveParams baseResolveParams;

		[CompilerGenerated]
		private sealed class <Generate>c__AnonStorey0
		{
			public <Generate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 v)
			{
				return v.Standable(this.map);
			}

			internal Map map;
		}
	}
}
