﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompWeatherSat : ThingComp
	{
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			Map map = this.parent.Map;
			GameConditionManager manager = map.gameConditionManager;
			if (this.mana > 10f)
			{
				list.Add(new FloatMenuOption(MenuOptionPriority.Default)
				{
					Label = "End extreme weather (10 mana).",
					action = delegate()
					{
						this.mana -= 10f;
						map.weatherManager.TransitionTo(WeatherDefOf.Clear);
						if (manager.ConditionIsActive(GameConditionDefOf.ColdSnap))
						{
							manager.ActiveConditions.Remove(manager.GetActiveCondition(GameConditionDefOf.ColdSnap));
						}
						if (manager.ConditionIsActive(GameConditionDefOf.Flashstorm))
						{
							manager.ActiveConditions.Remove(manager.GetActiveCondition(GameConditionDefOf.Flashstorm));
						}
						if (manager.ConditionIsActive(GameConditionDefOf.HeatWave))
						{
							manager.ActiveConditions.Remove(manager.GetActiveCondition(GameConditionDefOf.HeatWave));
						}
					}
				});
			}
			if (this.mana > 15f)
			{
				list.Add(new FloatMenuOption(MenuOptionPriority.Default)
				{
					Label = "Bring rain (15 mana)",
					action = delegate()
					{
						this.mana -= 15f;
						map.weatherManager.TransitionTo(WeatherDef.Named("Rain"));
					}
				});
			}
			if (this.mana > 18f)
			{
				list.Add(new FloatMenuOption(MenuOptionPriority.Default)
				{
					Label = "Bring fog (18 mana)",
					action = delegate()
					{
						this.mana -= 18f;
						map.weatherManager.TransitionTo(WeatherDef.Named("Fog"));
					}
				});
			}
			if (this.mana > 40f)
			{
				list.Add(new FloatMenuOption(MenuOptionPriority.Default)
				{
					Label = "Strike our enemies (40 mana)",
					action = delegate()
					{
						IEnumerable<Pawn> source = from p in map.mapPawns.AllPawnsSpawned
						where p.HostileTo(Faction.OfPlayer)
						select p;
						this.mana -= 40f;
						if (source.Count<Pawn>() > 0)
						{
							GameCondition_TargetedStorm gameCondition_TargetedStorm = (GameCondition_TargetedStorm)GameConditionMaker.MakeCondition(GameConditionDef.Named("TargetedStorm"), 12000, 1000);
							map.gameConditionManager.RegisterCondition(gameCondition_TargetedStorm);
						}
					}
				});
			}
			return list;
		}

		public override string CompInspectStringExtra()
		{
			return string.Format("Mana: {0:##.0}", this.mana);
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.mana, "link", 0f, false);
		}

		public float mana = 0f;
	}
}