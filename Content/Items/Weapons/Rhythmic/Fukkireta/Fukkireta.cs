//using Epicosity.Projectiles.Weapons.Ranged;
//using Epicosity.UI.Rhythm;
//using Epicosity.Utilities;
//using Microsoft.Xna.Framework;

//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using static Epicosity.UI.Rhythm.RhythmHandler;


// RS: This was commented out before I ported it.
// Someone wanna try restoring it?
namespace EpicosityPort.Content.Items.Weapons.Rhythmic.Fukkireta
{
	//public class Fukkireta : RhythmicWeaponBase
 //   {
 //       public override string Texture => PathConstants.PlaceholderTexture;
 //       public class NiconicoComment : RhythmicProjectile
 //       {
 //           bool hasHit = false;
 //           public override string Texture => "Epicosity/UI/Rhythm/NoteSkins/Note";
 //           public override void SetStaticDefaults()
 //           {
 //               DisplayName.SetDefault("Niconico Comment");
 //               ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
 //               ProjectileID.Sets.TrailingMode[projectile.type] = 2;
 //           }
 //           public override void SetDefaults()
 //           {
 //               projectile.aiStyle = 1;
 //               projectile.width = 32;
 //               projectile.height = 32;
 //               projectile.friendly = true;
 //               projectile.tileCollide = false;
 //               projectile.timeLeft = 120;
 //           }
 //           public override void AI()
 //           {
 //               if (hasHit)
 //               {
 //                   projectile.velocity = Vector2.Zero;
 //                   projectile.aiStyle = -1;
 //                   projectile.scale += 0.02f;
 //                   projectile.alpha += 20;
 //                   if (projectile.alpha >= 255)
 //                   {
 //                       projectile.alpha = 255;
 //                       projectile.Kill();
 //                   }
 //               }
 //               if ((Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height) - projectile.velocity) != Vector2.Zero && projectile.timeLeft < 60) 
 //               {
 //                   hasHit = true;
 //               }
 //           }
 //           public override bool OnTileCollide(Vector2 oldVelocity)
 //           {
 //               hasHit = true;
 //               return false;
 //           }
 //           public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
 //           {
 //               hasHit = true;
 //           }
 //           public override void OnHitPvp(Player target, int damage, bool crit)
 //           {
 //               hasHit = true;
 //           }
 //       }
 //       public override void SetStaticDefaults()
 //       {
 //           DisplayName.SetDefault("Fukkireta");
 //       }
 //       public override void SetRhythmistDefaults()
 //       {
 //           item.damage = 130;
 //           item.shoot = ModContent.ProjectileType<NiconicoComment>();
 //           item.shootSpeed = 20;
 //           item.useTime = 10;
 //           item.melee = true;
 //       }

 //       public override RhythmHandler.GameplayType GetGameplayType() => GameplayType.Swipe;

 //       public override double GetTrackSpeed() => 20;
	//	public override int NoteRate => 15;
	//	public override float GetComboDepletionRate() => 0.001f;
 //       public override MouseTrackPatternObject ForcedMouseTrackPattern => GetMouseTrackPattern<MouseFukkireta>();
 //   }
}
