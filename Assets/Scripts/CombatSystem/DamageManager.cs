using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unstable;

namespace CombatSystem
{
    public struct DamageIdPair
    {
        public int Id;
        public Damage Damage;
    }

    public static class DamageManager
    {
        private class DamageRecord
        {
            public Damage damage;
            public Dictionary<IDamageTaker, float> invincibilityFrames = new();
        }

        private static Dictionary<int, DamageRecord> _damages;
        private static List<IDamageTaker> _damageTakers;
        private static int _nextDamageId;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _damages = new Dictionary<int, DamageRecord>();
            _nextDamageId = 0;
            _removeInvincibilityFrameBuffer = new List<IDamageTaker>();
            _damageTakers = new List<IDamageTaker>();
        }

        public static void GetAllDamages(List<DamageIdPair> outList)
        {
            outList.Clear();
            foreach (var (id, rec) in _damages)
            {
                outList.Add(new DamageIdPair { Id = id, Damage = rec.damage });
            }
        }

        public static int SetDamage(Damage damage, int id = -1)
        {
            if (id == -1)
            {
                id = ++_nextDamageId;
            }

            if (!_damages.TryGetValue(id, out var damageRec))
            {
                damageRec = new DamageRecord();
            }

            damageRec.damage = damage;
            _damages[id] = damageRec;
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

        /// <summary>
        /// Record that <paramref name="damageTaker"/> would be invincible to
        /// <paramref name="dmgId"/> for <paramref name="newFrame"/> amount of time.
        /// </summary>
        /// <param name="damageTaker">The damage taker</param>
        /// <param name="dmgId">The damage that is ignored by <paramref name="damageTaker"/></param>
        /// <param name="newFrame">The amount of time to ignore the damage</param>
        /// <returns>False if <paramref name="damageTaker"/> is already invincible to <paramref name="dmgId"/>
        /// or <paramref name="dmgId"/> is invalid. True otherwise.</returns>
        public static bool SetInvincibilityFrame(IDamageTaker damageTaker, int dmgId, float newFrame)
        {
            if (!_damages.TryGetValue(dmgId, out var damageRec))
            {
                return false;
            }

            if (!damageRec.invincibilityFrames.ContainsKey(damageTaker))
            {
                damageRec.invincibilityFrames[damageTaker] = newFrame;
                return true;
            }

            return false;
        }

        private static List<IDamageTaker> _removeInvincibilityFrameBuffer;

        /// <summary>
        /// Subtracts <paramref name="deltaTime"/> from all recorded invincibility frames,
        /// and removes any frame that has expired.
        /// </summary>
        /// <param name="deltaTime">The amount of time to subtract</param>
        public static void TickInvincibilityFrames(float deltaTime)
        {
            foreach (var damageRec in _damages.Values)
            {
                foreach (var damageTaker in damageRec.invincibilityFrames.Keys.ToList())
                {
                    if ((damageRec.invincibilityFrames[damageTaker] -= deltaTime) <= 0.0f)
                    {
                        _removeInvincibilityFrameBuffer.Add(damageTaker);
                    }
                }

                foreach (var damageTaker in _removeInvincibilityFrameBuffer)
                {
                    damageRec.invincibilityFrames.Remove(damageTaker);
                }

                _removeInvincibilityFrameBuffer.Clear();
            }
        }

        private static readonly List<DamageIdPair> DamageBuffer = new();
        public static void AddDamageTaker(IDamageTaker damageTaker)
        {
            Debug.Assert(!_damageTakers.Contains(damageTaker), "!_damageTakers.Contains(damageTaker)");
            
            _damageTakers.Add(damageTaker);
        }
        
        public static void ResolveDamages()
        {
            GetAllDamages(DamageBuffer);

            foreach (var damageTaker in _damageTakers)
            {
                foreach (var pair in DamageBuffer)
                {
                    damageTaker.HandleDamage(pair.Id, pair.Damage);
                }
            }

            foreach (var damageTaker in _damageTakers)
            {
                damageTaker.ReckonAllDamage();
            }

            DamageBuffer.Clear();

            _damageTakers.RemoveAll(damageTaker => damageTaker.Dead);
        }
    }
}