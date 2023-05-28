using MelonLoader;
using BTD_Mod_Helper;
using DartMonkeyFourthPath;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using JetBrains.Annotations;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System.Runtime.CompilerServices;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

[assembly: MelonInfo(typeof(DartMonkeyFourthPath.DartMonkeyFourthPath), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace DartMonkeyFourthPath;

public class DartMonkeyFourthPath : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<DartMonkeyFourthPath>("DartMonkeyFourthPath loaded!");
    }

    public class FourthPathDart : PathPlusPlus
    {
        public override string Tower => TowerType.DartMonkey;
        public override int UpgradeCount => 5;

    }
    public class ReinforcedDarts : UpgradePlusPlus<FourthPathDart>
    {
        public override int Cost => 450;
        public override int Tier => 1;
        public override string Icon => VanillaSprites.SharperDartsUpgradeIcon;

        public override string Description => "Reinforced darts are durable enough to pop lead. ";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }
    }
    public class HomingDarts : UpgradePlusPlus<FourthPathDart>
    {
        public override int Cost => 650;
        public override int Tier => 2;
        public override string Icon => VanillaSprites.NevaMissTargetingUpgradeIcon;

        public override string Description => "Darts home on to bloons very agressively.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();

            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 5.0f;
            attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
            attackModel.weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().lifespan *= 5f;
        }
    }
    public class DartStorm : UpgradePlusPlus<FourthPathDart>
    {
        public override int Cost => 3500;
        public override int Tier => 3;
        public override string Icon => VanillaSprites.TackSprayerUpgradeIcon;

        public override string Description => "Shoots much more faster, and shoots 4 darts at at time.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 4, 0.0f, 35.0f, null, false);
            attackModel.weapons[0].rate *= 0.3f;
        }
    }
    public class TheDartZone: UpgradePlusPlus<FourthPathDart>
    {
        public override int Cost => 8000;
        public override int Tier => 4;
        public override string Icon => VanillaSprites.LotsMoreDartsUpgradeIcon;

        public override string Description => "Shoots 6 darts at a time, with more pierce and damage.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 6, 0.0f, 35.0f, null, false);
            attackModel.weapons[0].rate *= 0.3f;
        }
    }
    public class TheDartVortex : UpgradePlusPlus<FourthPathDart>
    {
        public override int Cost => 25000;
        public override int Tier => 5;
        public override string Icon => VanillaSprites.TheTackZoneUpgradeIcon;

        public override string Description => "Shoots a cluster dart with ALOT OF PIERCE. The more bloons, the more darts!";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 2, 0.0f, 35.0f, null, false);
            attackModel.weapons[0].projectile.GetDamageModel().damage += 5;
            attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", attackModel.weapons[0].projectile.Duplicate(), new ArcEmissionModel("ArcEmissionModel_", 2, 0.0f, 360.0f, null, false), true, false, false));
            attackModel.weapons[0].projectile.pierce += 250;
        }
    }
}