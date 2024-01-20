using System.Collections.Generic;

namespace Behaviour
{
    public static class RoundDTO
    {        
        private static Dictionary<MonsterBehaviour, bool> roundMonsters = new Dictionary<MonsterBehaviour, bool>();

        public static Dictionary<MonsterBehaviour, bool> RoundMonsters { get => roundMonsters; set => roundMonsters = value; }

        public static void TryAddMosnter(MonsterBehaviour monster)
        {
            if (RoundMonsters != null && !RoundMonsters.ContainsKey(monster))
                RoundMonsters.Add(monster, false);
        }

    }
}
