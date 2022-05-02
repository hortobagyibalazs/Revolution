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
        private long i = 0;
        private long frameDelay = 160;

        public void Update(int deltaMs)
        {
            i += deltaMs;
            if (i < frameDelay) return;
            i = 0;

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
