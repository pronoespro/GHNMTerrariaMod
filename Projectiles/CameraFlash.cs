﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Projectiles
{
    public class CameraFlash : ModProjectile
    {
        private Vector2 _targetPos;         //Ending position of the laser beam
        private int _charge;                //The charge level of the weapon
        private int _chargeSpeed = 10;
        private float _moveDist = 25f;       //The distance charge particle from the player center
        private float _duration = 15f;
        private Color _projColor = new Color(255, 255, 255, 0);
        private int filmType = -1;

        public override void SetDefaults()
        {
            projectile.Name = "flash";
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;     //this defines if the projectile is frendly
            projectile.penetrate = -1;  //this defines the projectile penetration, -1 = infinity
            projectile.tileCollide = false;   //this defines if the tile can colide with walls

            projectile.hide = true;
            projectile.alpha = 100;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (_charge == 100)
            {
                Vector2 unit = _targetPos - Main.player[projectile.owner].Center;
                unit.Normalize();
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center, unit, 5, projectile.damage, -1.57f, 1f, 1000f, _projColor, 45);// this is the projectile sprite draw, 45 = the distance of where the projectile starts from the player
            }
            return false;

        }

        /// <summary>
        /// The core function of drawing a laser
        /// </summary>
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            Color c = Color.White;
            if (filmType == 4)
            {
                c = Color.Blue;
            }
            else if (filmType == 3)
            {
                c = Color.Green;
            }
            else if (filmType == 2)
            {
                c = Color.Red;
            }
            else if (filmType == 1)
            {
                c = Color.Yellow;
            }

            #region Draw laser body
            for (float i = transDist; i <= _moveDist; i += step)
            {
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), c, r, new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (_moveDist + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), c, r, new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            #endregion
        }

        /// <summary>
        /// Change the way of collision check of the projectile
        /// </summary>
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (_charge == 100)
            {
                Player p = Main.player[projectile.owner];
                Vector2 unit = (Main.player[projectile.owner].Center - _targetPos);
                unit.Normalize();
                float point = 0f;
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), p.Center - 95f * unit, p.Center - unit * _moveDist, 22, ref point))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Change the behavior after hit a NPC
        /// </summary>
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }


        /// <summary>
        /// The AI of the projectile
        /// </summary>
        public override void AI()
        {

            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[projectile.owner];

            #region consume films
            if (filmType < 0)
            {
                filmType = 0;
                if (player.HasItem(mod.ItemType("FilmZ")))
                {
                    projectile.damage *= 16;
                    player.ConsumeItem(mod.ItemType("FilmZ"), true);
                    filmType = 1;
                }
                else if (player.HasItem(mod.ItemType("Film74")))
                {
                    projectile.damage *= 8;
                    player.ConsumeItem(mod.ItemType("Film74"), true);
                    filmType = 2;
                }
                else if (player.HasItem(mod.ItemType("Film37")))
                {
                    projectile.damage *= 4;
                    player.ConsumeItem(mod.ItemType("Film37"), true);
                    filmType = 3;
                }
                else if (player.HasItem(mod.ItemType("Film14")))
                {
                    projectile.damage *= 2;
                    player.ConsumeItem(mod.ItemType("Film14"), true);
                    filmType = 4;
                }
            }
            #endregion


            #region Set projectile position
            if (projectile.owner == Main.myPlayer) // Multiplayer support
            {
                Vector2 diff = mousePos - player.Center;
                diff.Normalize();
                projectile.position = player.Center + diff * _moveDist;
                projectile.timeLeft = 2;
                int dir = projectile.position.X > player.position.X ? 1 : -1;
                player.ChangeDir(dir);
                player.heldProj = projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = (float)Math.Atan2(diff.Y * dir, diff.X * dir);
                projectile.soundDelay--;
            }
            #endregion

            #region Charging process
            // Kill the projectile if the player stops channeling
            if (!player.channel || _duration < 0)
            {
                projectile.Kill();
            }
            else
            {
                if (Main.time % 10 < 1 && !player.CheckMana(player.inventory[player.selectedItem].mana, true))
                {
                    projectile.Kill();
                }
                Vector2 offset = mousePos - player.Center;
                offset.Normalize();
                offset *= _moveDist - 20;
                Vector2 dustPos = player.Center + offset - new Vector2(10, 10);
                if (_charge < 100)
                {
                    _charge += _chargeSpeed;
                }
                else
                {
                    _duration--;
                }
                int chargeFact = _charge / 20;
                Vector2 dustVelocity = Vector2.UnitX * 18f;
                dustVelocity = dustVelocity.RotatedBy(projectile.rotation - 1.57f, default(Vector2));
                Vector2 spawnPos = projectile.Center + dustVelocity;
                for (int k = 0; k < chargeFact + 1; k++)
                {
                    Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - (chargeFact * 2));
                    Dust dust = Main.dust[Dust.NewDust(dustPos, 30, 30, 235, projectile.velocity.X / 2f,    //this 30, 30 is the dust weight and height 235 is the tail dust    
                        projectile.velocity.Y / 2f, 0, default(Color), 1f)];
                    dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - chargeFact * 2f) / 10f;
                    dust.noGravity = true;
                    dust.scale = Main.rand.Next(10, 20) * 0.05f;
                }
            }
            #endregion


            #region Set laser tail position and dusts
            if (_charge < 100) return;
            Vector2 start = player.Center;
            Vector2 unit = (player.Center - mousePos);
            unit.Normalize();
            unit *= -1;
            for (_moveDist = 95f; _moveDist <= 1600; _moveDist += 5) //this 1600 is the dsitance of the beam
            {
                start = player.Center + unit * _moveDist;
                if (!Collision.CanHit(player.Center, 1, 1, start, 1, 1))
                {
                    _moveDist -= 5f;
                    break;
                }

                if (projectile.soundDelay <= 0)//this is the proper sound delay for this type of weapon
                {
                    Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15);    //this is the sound when the weapon is used   cheange 15 for diferent sound
                    projectile.soundDelay = 40;    //this is the proper sound delay for this type of weapon
                }

            }
            _targetPos = player.Center + unit * _moveDist;

            //dust
            for (int i = 0; i < 2; ++i)
            {
                float num1 = projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Main.dust[Dust.NewDust(_targetPos, 0, 0, 235, dustVel.X, dustVel.Y, 0, new Color(), 1f)];  //this is the head dust
                Dust dust2 = Main.dust[Dust.NewDust(_targetPos, 0, 0, 235, dustVel.X, dustVel.Y, 0, new Color(), 1f)]; //this is the head dust 2
                dust.noGravity = true;
                dust.scale = 1.2f;
            }

            #endregion
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }

}
