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
            
            foreach (var entity in EntityManager.GetEntities())
            {
                var movementComp = entity.GetComponent<MovementComponent>();
                if (movementComp != null && (movementComp.TargetTileDeltaX != 0 || movementComp.TargetTileDeltaY != 0))
                {
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