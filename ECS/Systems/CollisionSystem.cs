using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class CollisionSystem : ISystem
    {
        public void Update(int deltaMs)
        {
            foreach(var entity in EntityManager.GetEntities())
            {
                foreach(var entity2 in EntityManager.GetEntities())
                {
                    if (entity != entity2)
                    {
                        var entityOneCollisionComp = entity.GetComponent<CollisionComponent>();
                        var entityTwoCollisionComp = entity2.GetComponent<CollisionComponent>();

                        if (entityOneCollisionComp.CollidesWith(entityTwoCollisionComp))
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
