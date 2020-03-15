using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_CrashedShip : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return base.CanFireNowSub(parms) && TileFinder.TryFindNewSiteTile(out num);
		}

		public List<Thing> GenerateRewards()
		{
            ThingSetMakerParams value = default(ThingSetMakerParams);
            value.techLevel = TechLevel.Spacer;
            value.countRange = new IntRange?(new IntRange(1, 1));
            value.totalMarketValueRange = new FloatRange?(new FloatRange(500f, 3000f));
            return ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(value);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			int tile;
			if (map == null)
			{
				result = false;
			}
			else if (!TileFinder.TryFindNewSiteTile(out tile))
			{
				result = false;
			}
			else
			{
				bool flag = false;
				bool flag2 = true;
				IEnumerable<Pawn> source = from p in map.mapPawns.FreeColonistsSpawned
				where p.CurJob.def == JobDefOfReconAndDiscovery.Skygaze || p.CurJob.def == JobDefOfReconAndDiscovery.UseTelescope
				select p;
				Pawn pawn = null;
				if (map.listerBuildings.ColonistsHaveBuilding(ThingDef.Named("CommsConsole")))
				{
					if (Rand.Chance(0.5f))
					{
						flag = true;
					}
				}
				else if (source.Count<Pawn>() > 0)
				{
					flag = true;
					flag2 = false;
					pawn = source.RandomElement<Pawn>();
				}
				if (!flag)
				{
					result = false;
				}
				else
				{
					bool flag3 = Rand.Value < 0.4f;
					Site site = SiteMaker.MakeSite(SiteDefOfReconAndDiscovery.CrashedShip, tile, 
                        Find.FactionManager.RandomEnemyFaction(true, false, true, TechLevel.Spacer));
                    // not sure what to do with all these
                    //if (flag3)
                    //{
                    //	site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.AdventureMedical);
                    //}
                    //else
                    //{
                    //	site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
                    //}
					if (flag3)
					{
                        SitePart medicalEmergency = new SitePart(site, SiteDefOfReconAndDiscovery.MedicalEmergency, null);
                        site.parts.Add(medicalEmergency);
						QuestComp_MedicalEmergency component = site.GetComponent<QuestComp_MedicalEmergency>();
						component.parent = site;
						component.Initialize(new WorldObjectCompProperties_MedicalEmergency());
						component.maxPawns = Rand.RangeInclusive(3, 12);
						List<Thing> rewards = this.GenerateRewards();
						component.StartQuest(rewards);
					}
					else if (!Rand.Chance(0.75f))
					{
                        SitePart rareBeasts = new SitePart(site, SiteDefOfReconAndDiscovery.RareBeasts, null);

                        site.parts.Add(rareBeasts);
					}
					if (Rand.Value < 0.85f)
					{
                        SitePart scatteredTreasure = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredTreasure, null);

                        site.parts.Add(scatteredTreasure);
					}
					if (Rand.Value < 0.1f)
					{
                        SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, null);
                        site.parts.Add(scatteredManhunters);
					}
					if (Rand.Value < 0.1f)
					{
                        SitePart mechanoidForces = new SitePart(site, SiteDefOfReconAndDiscovery.MechanoidForces, null);
                        site.parts.Add(mechanoidForces);
					}
					if (Rand.Value < 0.5f)
					{
                        SitePart enemyRaidOnArrival = new SitePart(site, SiteDefOfReconAndDiscovery.EnemyRaidOnArrival, null);
                        site.parts.Add(enemyRaidOnArrival);
					}
					int randomInRange = IncidentWorker_CrashedShip.TimeoutDaysRange.RandomInRange;
					site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					if (flag2)
					{
						base.SendStandardLetter(parms, site);
					}
					else
					{
						Find.LetterStack.ReceiveLetter("Shooting star", string.Format("{0} just saw something fall from the sky near here!", pawn.Label), LetterDefOf.PositiveEvent, site, null);
					}
					result = true;
				}
			}
			return result;
		}

		private static readonly IntRange TimeoutDaysRange = new IntRange(6, 10);
	}
}
