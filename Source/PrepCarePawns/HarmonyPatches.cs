/*
 * Created with SharpDevelop.
 * Author: Hikitsune-Red
 * Date: 6/13/2018
 * 
 */
 
using System;
using System.Linq;
using System.Reflection;
using Verse;
using RimWorld;
using RimWorld.Planet;
using Harmony;

namespace PrepCarePawns
{
	[StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.hkr.prepcarepawns");
            
            MethodInfo pawnTickMethod = AccessTools.Method(typeof(Verse.Pawn),"Tick");
            
            HarmonyMethod pawnTickPostfix = new HarmonyMethod(typeof(PrepCarePawns.HarmonyPatches).GetMethod("PawnTick_Postfix"));
            
            harmony.Patch(pawnTickMethod, null, pawnTickPostfix);
        }
        
        public static void PawnTick_Postfix(Verse.Pawn __instance)
        {
        	if (__instance.Spawned && !__instance.NonHumanlikeOrWildMan() && __instance.story.traits.HasTrait(TraitDef.Named("HKR_SpawnInWorld")))
        	{
        		__instance.story.traits.allTraits.Remove(__instance.story.traits.GetTrait(TraitDef.Named("HKR_SpawnInWorld")));
        		__instance.DeSpawn();
				Find.WorldPawns.PassToWorld(__instance, PawnDiscardDecideMode.KeepForever);
        	}
        }
    }
}