using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using System.Windows.Input;

namespace Revolution.ECS.Systems
{
    public class MovementSystem : ISystem
    {
        public void Update(int deltaMs)
        {
            if (Keyboard.IsKeyDown(Key.V))
            {
                var villager = EntityManager.CreateEntity<Villager>();
            }
            
            // Movement
            if (Keyboard.IsKeyDown(Key.D))
            {
                foreach (var entity in EntityManager.GetEntities())
                {
                    var movementComp = entity.GetComponent<MovementComponent>();
                    var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                    if (movementComp != null)
                    {
                        movementComp.VelocityX = 4;
                        movementComp.DestinationTileX = gameMapObjectComp.X + 1;
                        movementComp.DestinationTileY = gameMapObjectComp.Y;    
                    }
                }
            } 
            else if (Keyboard.IsKeyDown(Key.A))
            {
                foreach (var entity in EntityManager.GetEntities())
                {
                    var movementComp = entity.GetComponent<MovementComponent>();
                    var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                    if (movementComp != null)
                    {
                        movementComp.VelocityX = -4;
                        movementComp.DestinationTileX = gameMapObjectComp.X - 1;
                        movementComp.DestinationTileY = gameMapObjectComp.Y;
                    }
                }
            }
            else if (Keyboard.IsKeyDown(Key.W))
            {
                foreach (var entity in EntityManager.GetEntities())
                {
                    var movementComp = entity.GetComponent<MovementComponent>();
                    var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                    if (movementComp != null)
                    {
                        movementComp.VelocityY = -4;
                        movementComp.DestinationTileX = gameMapObjectComp.X;
                        movementComp.DestinationTileY = gameMapObjectComp.Y - 1;
                    }
                }
            }
            else if (Keyboard.IsKeyDown(Key.S))
            {
                foreach (var entity in EntityManager.GetEntities())
                {
                    var movementComp = entity.GetComponent<MovementComponent>();
                    var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                    if (movementComp != null)
                    {
                        movementComp.VelocityY = 4;
                        movementComp.DestinationTileX = gameMapObjectComp.X;
                        movementComp.DestinationTileY = gameMapObjectComp.Y + 1;
                    }
                }
            }

            foreach (var entity in EntityManager.GetEntities())
            {
                var movementComp = entity.GetComponent<MovementComponent>();
                var gameObjectComp = entity.GetComponent<GameMapObjectComponent>();
                if (movementComp != null && gameObjectComp != null)
                {
                    if ((movementComp.DestinationTileX == gameObjectComp.X &&
                    movementComp.DestinationTileY == gameObjectComp.Y))
                    {
                        movementComp.Stop();
                        return;
                    }

                    var collisionComp = entity.GetComponent<CollisionComponent>();
                    bool collides = false;
                    if (collisionComp != null)
                    {
                        collides = EntityCollides(entity);
                    }

                    if (!collides)
                    {
                        var posComp = entity.GetComponent<PositionComponent>();
                        posComp.X += movementComp.VelocityX;
                        posComp.Y += movementComp.VelocityY;
                    } 
                    else
                    {
                        movementComp.Stop();
                    }
                }
            }
        }

        private bool EntityCollides(Entity entity)
        {
            var collisionComp = entity.GetComponent<CollisionComponent>();
            foreach (var entity2 in EntityManager.GetEntities())
            {
                var collisionComp2 = entity2.GetComponent<CollisionComponent>();
                if (entity2 != entity && collisionComp2 != null &&
                    collisionComp.CollidesWith(collisionComp2))
                {
                    return true;
                }
            }

            return false;
        }
    }
}