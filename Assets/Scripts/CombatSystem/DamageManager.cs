using System;
using System.Collections.Generic;
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
        private static int _nextDamageId;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _damages = new Dictionary<int, DamageRecord>();
            _nextDamageId = 0;
            _removeInvincibilityFrameBuffer = new List<IDamageTaker>();
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

        public static bool UpdateInvincibilityFrame(IDamageTaker damageTaker, int dmgId, float newFrame)
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

        public static void TickInvincibilityFrames(float deltaTime)
        {
            foreach (var damageRec in _damages.Values)
            {
                foreach (var damageTaker in damageRec.invincibilityFrames.Keys)
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
    }
}