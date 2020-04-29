using GodsHaveNoMercy.Logic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace GodsHaveNoMercy
{

    public class DarkPlayer : ModPlayer
    {
        public DarknessUI Logic { get; private set; }

        public bool summonShadowMinion = false;
        public bool ZoneHauntedMansion = false;
        public Vector2 offset = new Vector2(50, 0);
        public bool ApearedBehind = false;
        public int TotalDarkness = 50;
        public int MaxDarkness = 50;
        public int buffedDarkness = 0;
        public int maxDarkDmg = 150;
        public int shadowGuardianDamage = 150;
        public bool doubleDarkDamage = false;
        public float DarknessUseTimer = 10;
        public bool lastUseDash = false;
        public int darknessRecovery = 1;

        public WeaponLevelSystem shadowPunch = new WeaponLevelSystem();
        public WeaponLevelSystem darkDash = new WeaponLevelSystem();
        public int ShadowGuardianAttack = 0;
        public bool UltimateAttackPunch = false;
        public bool UltimateAttackDash = false;

        private float apearedTimer = 6000;
        private Vector2 lastPos = new Vector2();

        public float CleansingDamage = 1f;

        int tpProjectile = 0;
        int reviveTime = 0;

        public override void UpdateBiomes()
        {
            ZoneHauntedMansion = (DarkWorld.HauntedMansion > 3);
        }

        public override void ResetEffects()
        {
            darknessRecovery = 1;
            summonShadowMinion = false;
            //mod.Instance.DarkInterface.SetState(null);
            doubleDarkDamage = false;
            CleansingDamage = 1f;
            MaxDarkness = 50;
        }

        public void BuffDarkness(int ammount)
        {
            if (buffedDarkness < maxDarkDmg - ammount)
            {
                buffedDarkness += ammount;
            }
            else
            {
                buffedDarkness += maxDarkDmg - buffedDarkness;
            }
        }

        public void ResetApearTimer()
        {
            apearedTimer = 6000;
        }

        public bool CostDarkness(int cost, bool dash)
        {
            lastUseDash = dash;
            if (DarknessUseTimer < 0)
            {
                if (TotalDarkness > cost)
                {
                    TotalDarkness -= cost;
                }
                else if (TotalDarkness > cost / 2)
                {
                    TotalDarkness = 0;
                }
                else
                {
                    return false;
                }
                DarknessUseTimer = 50;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RecoverDarkness(int ammount)
        {
            if (TotalDarkness == MaxDarkness)
                return false;

            if (TotalDarkness + ammount > MaxDarkness)
            {
                TotalDarkness = MaxDarkness;
            }
            else
            {
                TotalDarkness += ammount;
            }
            return true;
        }

        private void TP(NPC npc)
        {
            if (Main.projectile[tpProjectile].type == mod.ProjectileType("teleport_projectile"))
                Main.projectile[tpProjectile].timeLeft = 0;

            tpProjectile = Projectile.NewProjectile(player.position, new Vector2(), mod.ProjectileType("teleport_projectile"), 0, 0);
            Vector2 turnedOffset = offset * -npc.spriteDirection;
            lastPos = player.position;
            player.position = npc.position + turnedOffset;
            ApearedBehind = true;
            apearedTimer = 6000;
        }


        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            if (player.HasItem(mod.ItemType("StoneMirror")) && reviveTime <= 0)
            {
                player.ConsumeItem(mod.ItemType("StoneMirror"));
                player.statLife = player.statLifeMax2;
                player.HealEffect(player.statLifeMax2);
                player.immune = true;
                player.immuneTime = player.longInvince ? 180 : 120;
                for (int k = 0; k < player.hurtCooldowns.Length; k++)
                {
                    player.hurtCooldowns[k] = player.longInvince ? 180 : 120;
                }
                Main.PlaySound(SoundID.Item29, player.position);
                reviveTime = 60 * 60 * 3;
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            reviveTime = 0;
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (Main.rand.Next(5) == 0 && !npc.boss && player.ownedProjectileCounts[mod.ProjectileType("ShadowGuardian")] > 0)
            {
                if (CostDarkness(1, false))
                {
                    TP(npc);
                }
            }
            else if (TotalDarkness < 50)
            {
                TotalDarkness++;
            }
            base.OnHitByNPC(npc, damage, crit);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (GodsHaveNoMercy.TeleportBackHotKey.JustPressed)
            {
                if (ApearedBehind)
                {
                    if (!player.dead)
                    {
                        if (Main.projectile[tpProjectile].type == mod.ProjectileType("teleport_projectile"))
                            Main.projectile[tpProjectile].timeLeft = 0;
                        player.position = lastPos;
                        apearedTimer = 0;
                    }
                    if (apearedTimer <= 0)
                    {
                        ApearedBehind = false;
                        lastPos = new Vector2();
                    }
                }
            }

            //shadow guardian quick use
            if (GodsHaveNoMercy.useShadowReleaser.JustPressed)
            {

                for (int i = 0; i < player.inventory.Length; i++)
                {
                    Item item = player.inventory[i];

                    if (item.stack > 0 && item.Name == "Shadow releaser" && !summonShadowMinion)
                    {
                        QuickUseItem(item, i);
                    }
                }
            }

            //Ultimate attack!!!!!
            if (GodsHaveNoMercy.useShadowPunchDash.JustPressed)
            {

                for (int i = 0; i < player.inventory.Length; i++)
                {
                    Item item = player.inventory[i];
                    bool hasShadowGuardian = false;
                    for (int p = 0; p < Main.projectile.Length; p++)
                    {
                        if (Main.projectile[p].type == mod.ProjectileType("ShadowGuardian"))
                        {
                            hasShadowGuardian = true;
                            break;
                        }
                    }
                    if (item.stack > 0 && item.Name == "Shadow punch" && hasShadowGuardian)
                    {
                        //QuickUseItem(item, i,true);
                        if (darkDash.GetUltimate())
                        {
                            UltimateAttackDash = true;
                        }
                        else if (shadowPunch.GetUltimate())
                        {
                            UltimateAttackPunch = true;
                        }
                    }
                }

            }

            base.ProcessTriggers(triggersSet);
        }

        public override void PreUpdate()
        {

            DarknessUseTimer--;
            if (reviveTime > 0)
                reviveTime--;

            base.PreUpdate();
        }

        private void QuickUseItem(Item item, int index, bool altFunction = false)
        {
            int prevSelItem = player.selectedItem;
            player.selectedItem = index;

            if (altFunction)
            {
                player.releaseUseItem = false;
                player.controlUseItem = true;
            }
            else
            {
                player.controlUseItem = true;
            }

            player.ItemCheck(Main.myPlayer);

            player.selectedItem = prevSelItem;

            return;

        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"Darkness",TotalDarkness},
               // {"WeaponLevel_0", weaponLevels[0].level},
               // { "WeaponLevel_1", weaponLevels[1].level}
            };
        }

        public override void Load(TagCompound tag)
        {
            shadowPunch.SetDefault("Shadow Punch", expToFLvl: 15);
            darkDash.SetDefault("DarkDash", expToFLvl: 5, maxlvl: 2);

            //TotalDarkness = tag.GetInt("Darkness");
        }

    }
}
