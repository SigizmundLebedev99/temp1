// using System;
// using DefaultEcs;
// using DefaultEcs.System;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Content;
// using temp1.Components;

// namespace temp1.Systems
// {
//     class SpawnSystem : AEntitySetSystem<GameTime>
//     {
//         bool spawned = false;
        
//         public SpawnSystem(World world) : base(world)
//         {
//         }

//         public override void Update(GameTime gameTime)
//         {
//             if (spawned)
//                 return;
            
//             var portal = CreateEntity();
//             var random = new Random();
//             var grid = _map.MovementGrid;
//             Point point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
//             if (!grid.IsWalkableAt(point.X, point.Y))
//                 return;
//             spawned = true;
//             portal.Attach(new MapObject
//             {
//                 Position = point.ToVector2() * 32 + new Vector2(16)
//             });
//             portal.Attach(new RenderingObject(_content.GetAnimatedSprite("images/portal.sf")));
//             portal.Attach<Expired>(new Timer(1.5f, () =>
//             {
//                 _gameObjects.CreateMapObject("enemy", point.ToVector2() * 32 + new Vector2(16));
//             }, true));
//         }
//     }
// }