using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class SpriteAnimationSystem : ISystem
    {
        private long i = long.MinValue;
        public void Update(int deltaMs)
        {
            if (i++ % 2 == 0) return;
            if (i == long.MaxValue) i = long.MinValue;

            foreach (var entity in EntityManager.GetEntities())
            {
                var animatedSpriteComp = entity.GetComponent<AnimatedSpriteComponent>();
                if (animatedSpriteComp != null)
                {
                    animatedSpriteComp.NextFrame();
                }
            }
        }
    }
}
