using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class PlayerResourceSystem : ISystem
    {
        public void Update(int deltaMs)
        {
            var player = PlayerHelper.GetGuiControlledPlayer();
            var playerTeamComp = player.GetComponent<TeamComponent>();
            var playerResourceComp = player.GetComponent<ResourceComponent>();

            playerResourceComp.Population = 0;
            playerResourceComp.MaxPopulation = 0;

            foreach (var entity in EntityManager.GetEntities())
            {
                var resourceComp = entity.GetComponent<ResourceComponent>();
                var teamComp = entity.GetComponent<TeamComponent>();

                if (teamComp != null && teamComp.TeamId == playerTeamComp.TeamId && resourceComp != null)
                {
                    if (entity is TownCenter)
                    {
                        playerResourceComp.Wood += resourceComp.Wood;
                        playerResourceComp.Gold += resourceComp.Gold;
                        resourceComp.Wood = 0;
                        resourceComp.Gold = 0;
                        playerResourceComp.MaxPopulation += resourceComp.MaxPopulation;
                    }
                    else if (entity is House)
                    {
                        playerResourceComp.MaxPopulation += resourceComp.MaxPopulation;
                    }
                    else
                    {
                        playerResourceComp.Population += resourceComp.Population;
                    }
                }
            }
        }
    }
}
