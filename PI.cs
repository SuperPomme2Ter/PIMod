using System;
using System.Collections.Generic;
using Alexandria.ItemAPI;
using Alexandria.VisualAPI;
using Alexandria.SoundAPI;
using Gungeon;
using UnityEngine;
using Random = UnityEngine.Random;
using HutongGames.PlayMaker.Actions;
using MonoMod.RuntimeDetour;
using System.Linq;


namespace PIWeapon
{
    internal class PI : AdvancedGunBehavior
    {
        public override void Start()
        {
            base.Start();
            if (gun.shootAnimation != null)
            {
                Plugin.Log($"Shooting animation found. path : {gun.shootAnimation}", "#FFFFFF");
                Plugin.Log($"Reload animation found. path : {gun.reloadAnimation}", "#FFFFFF");
                Plugin.Log($"damage {gun.DefaultModule.projectiles[0].baseData.damage}", "#FFFFFF");
                foreach (AdvancedSynergyEntry entry in GameManager.Instance.SynergyManager.synergies.Where(x => x.MandatoryGunIDs.Contains(175) || x.OptionalGunIDs.Contains(175)))
                {
                    foreach(CustomSynergyType synergy in entry.bonusSynergies)
                    {
                        Plugin.Log(synergy.ToString());
                    }
                }
            }
            else
            {
                Plugin.Log("shooting animation not found");
            }
            if (gun.sprite == null)
            {
                Plugin.Log("IDLE not found");
            }
            if (gun.shellCasing == null)
            {
                Plugin.Log("Casing not found");
            }
            projectilePath.Add("pi_projectile_001");
            projectilePath.Add("pi_projectile_002");
            projectilePath.Add("pi_projectile_003");
            projectilePath.Add("pi_projectile_004");
        }
        public static Gun Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("PIstol", "pi");
            Game.Items.Rename("outdated_gun_mods:pistol", "pimod:PIstol");
            gun.gameObject.AddComponent<PI>();
            string SpriteDirectory = "PIWeapon/Resources/CustomGunAmmoTypes/pi_ammo";
            string SpriteDirectoryDepleted= "PIWeapon/Resources/CustomGunAmmoTypes/pi_ammo_depleted";
            string SpriteDirectoryClip= "PIWeapon/Resources/PILEG.png";
            string SpriteDirectoryCasing = "PIWeapon/Resources/PICasing.png";
            //List<string> SpriteDirectoryMuzzleFlashEffect = new List<string>(1);
            //SpriteDirectoryMuzzleFlashEffect.Add("PIWeapon/Resources/referentiel.png");
            GunExt.SetupSprite(gun, null, "pi_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 30);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 10);        
            GunExt.SetShortDescription(gun, "360 no angles.");
            GunExt.SetLongDescription(gun, "Even though the letter PI does not look like a weapon, a succesful Gungoneer who help the inhabitants of the Breach thought it would be a good idea for a weapon.");
            GunExt.AddProjectileModuleFrom(gun, "magnum", false, true);
            //gun.m_originalBarrelOffsetPosition = new Vector3(1.25f, 0.65f, gun.barrelOffset.position.z);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "pi_idle_001";
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = Mathf.PI / 2;
            gun.DefaultModule.angleVariance = Mathf.PI;
            gun.DefaultModule.cooldownTime = 1 / (2*Mathf.PI);
            gun.DefaultModule.numberOfShotsInClip = 45;

            string PISwitchGroup = "pi_mod_PIstol";
            SoundManager.AddCustomSwitchData("WPN_Guns", gun.gunSwitchGroup, "Play_WPN_Gun_Shot_01",
                                "Play_WPN_zorgun_shot_01");
            SoundManager.AddCustomSwitchData("WPN_Guns", gun.gunSwitchGroup, "Play_WPN_Gun_Reload_01",
                                            "Play_WPN_makarov_reload_01");
            
            
            gun.muzzleFlashEffects = VFXBuilder.CreateVFXPool("PI Muzzleflash", new List<string>
            {
                "PIWeapon/Resources/MiscVFX/GunVFX/referentiel_muzzleflash_001",
                "PIWeapon/Resources/MiscVFX/GunVFX/referentiel_muzzleflash_002",
                "PIWeapon/Resources/MiscVFX/GunVFX/referentiel_muzzleflash_003",
                "PIWeapon/Resources/MiscVFX/GunVFX/referentiel_muzzleflash_004",
                "PIWeapon/Resources/MiscVFX/GunVFX/referentiel_muzzleflash_005",
                "PIWeapon/Resources/MiscVFX/GunVFX/referentiel_muzzleflash_006"
            },
            30,
            new IntVector2(20, 17),
            tk2dBaseSprite.Anchor.MiddleLeft,
            false,
            0,
            false,VFXAlignment.Fixed);
            gun.barrelOffset.transform.localPosition += new Vector3(0.1f, 0.15f, 0);
            gun.muzzleFlashEffects.SpawnAtPosition(gun.m_originalBarrelOffsetPosition);
            gun.SetBaseMaxAmmo(360);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "THE PI";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.SILLY;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("pi_projectile_001", 8, 8);
            projectile.baseData.damage =4*Mathf.PI;
            projectile.baseData.speed = 45;
            projectile.baseData.range = 45f;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>(), false, "ANY");
            //gun.shellCasing = (PickupObjectDatabase.GetById(15) as Gun).shellCasing; //Example using AK-47 casings.
            
            gun.shellsToLaunchOnFire = 1;
            gun.clipsToLaunchOnReload = 1;
            gun.clipObject=ItemBuilder.AddSpriteToObject("PILEG", SpriteDirectoryClip, gun.clipObject);
            gun.shellCasing = ItemBuilder.AddSpriteToObject("PICasing", SpriteDirectoryCasing, gun.shellCasing);
            
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("PI",
                SpriteDirectory,
                SpriteDirectoryDepleted);
            //gun.AddItemToSynergy(,5);
            //gun.AddItemToSynergy(CustomSynergyType.THREE_SIXTY_SCOPE,true);
            //gun.AddItemToSynergy(CustomSynergyType.FULLCIRCLE,true);
            //GameManager.Instance.SynergyManager.synergies;
            
            return gun;
        }
        protected override void Update()
        {
            base.Update();
            bool flag = this.gun.CurrentOwner;
            if (flag)
            {
                bool preventNormalFireAudio = this.gun.PreventNormalFireAudio;
                if (preventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                bool flag2 = !this.gun.IsReloading && !this.HasReloaded;
                if (flag2)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            gun.DefaultModule.projectiles[0].SetProjectileSpriteRight(projectilePath[Random.Range(0, projectilePath.Count)], 6, 6);

        }
        protected override void OnPickup(GameActor owner)
        {
            base.OnPickup(owner);
        }
        protected override void OnPostDrop(GameActor owner)
        {
            base.OnPostDrop(owner);
        }
        private bool HasReloaded;
        List<string> projectilePath = new List<string>();

    }
}
