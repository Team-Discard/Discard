using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public struct DamageIdPair
    {
        public int Id;
        public Damage Damage;
    }

    public static class DamageManager
    {
        private static Dictionary<int, Damage> _damages;
        private static int _nextDamageId;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _damages = new Dictionary<int, Damage>();
            _nextDamageId = 0;
        }

        public static void GetAllDamages(List<DamageIdPair> outList)
        {
            outList.Clear();
            foreach (var (id, damage) in _damages)
            {
                outList.Add(new DamageIdPair { Id = id, Damage = damage });
            }
        }

        public static int SetDamage(Damage damage, int id = -1)
        {
            if (id == -1)
            {
                id = ++_nextDamageId;
            }

            _damages[id] = damage;
            return id;
        }

        public static bool ClearDamage(int id)
        {
            return _damages.Remove(id);
        }

        public static void ClearDamage(ref int id)
        {
            if (!_damages.Remove(id))
            {
                throw new Exception($"Damage with {id} does not exist!");
            }
            
            id = -1;
        }
    }
}