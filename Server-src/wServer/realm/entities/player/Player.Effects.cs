using System;
using common.resources;

namespace wServer.realm.entities
{
    partial class Player
    {
        float _healing;
        float _bleeding;
        float _manaBurn;
        float _enchanted;

        int _newbieTime;
        int _canTpCooldownTime;
        int _canDecoyCooldownTime;
        void HandleEffects(RealmTime time)//Enchanted
        {
            var chr = _client.Character;

            if (_client.Account.Hidden && !HasConditionEffect(ConditionEffects.Hidden))
            {
                ApplyConditionEffect(ConditionEffectIndex.Hidden);
                ApplyConditionEffect(ConditionEffectIndex.Invincible);
                Manager.Clients[Client].Hidden = true;
            }
            
            if (Muted && !HasConditionEffect(ConditionEffects.Muted))
                ApplyConditionEffect(ConditionEffectIndex.Muted);

            if (HasConditionEffect(ConditionEffects.Healing) && !HasConditionEffect(ConditionEffects.Sick))
            {
                if (_healing > 1)
                {
                    HP = Math.Min(Stats[0], HP + (int)_healing);
                    _healing -= (int)_healing;
                }
                _healing += 28 * (time.ElaspedMsDelta / 1000f);
            }

            if (HasConditionEffect(ConditionEffects.Quiet) && MP > 0)
                MP = 0;
            
            if (HasConditionEffect(ConditionEffects.Bleeding) && HP > 1)
            {
                if (_bleeding > 1)
                {
                    HP -= (int)_bleeding;
                    if (HP < 1)
                        HP = 1;
                    _bleeding -= (int)_bleeding;
                }
                _bleeding += 28 * (time.ElaspedMsDelta / 1000f);
            }

            if (HasConditionEffect(ConditionEffects.ManaBurn) && MP > 0)
            {
                if (_manaBurn > 1)
                {
                    MP -= (int)_manaBurn;
                    if (MP < 0)
                        MP = 0;
                    _manaBurn -= (int)_manaBurn;
                }
                _manaBurn += 28 * (time.ElaspedMsDelta / 1000f);
            }

            if (HasConditionEffect(ConditionEffects.NinjaSpeedy))
            {
                MP = Math.Max(0, (int)(MP - 10 * time.ElaspedMsDelta / 1000f));

                if (MP == 0)
                    ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy, 0);
            }

            if (HasConditionEffect(ConditionEffects.NoDamage))
            {
                ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 500);
                MP = Math.Max(0, (int)(MP - 100 * time.ElaspedMsDelta / 1000f));

                if (MP == 0)
                {
                    ApplyConditionEffect(ConditionEffectIndex.NoDamage, 0);
                    ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 0);
                }
                    
            }

            // if (HasConditionEffect(ConditionEffects.Strength))
            // {

            //         ApplyConditionEffect(ConditionEffectIndex.Strength, 0);
            // }

            if (_newbieTime > 0)
            {
                _newbieTime -= time.ElaspedMsDelta;
                if (_newbieTime < 0) 
                    _newbieTime = 0;
            }

            if (_canTpCooldownTime > 0)
            {
                _canTpCooldownTime -= time.ElaspedMsDelta;
                if (_canTpCooldownTime < 0)
                    _canTpCooldownTime = 0;
            }

            if (_canDecoyCooldownTime > 0)
            {
                _canDecoyCooldownTime -= time.ElaspedMsDelta;
                if (_canDecoyCooldownTime < 0)
                    _canDecoyCooldownTime = 0;
            }

            if (Client.Player.Owner.SafeMapArea == true)
            {
                if (chr.MoonTime > 0)
                {
                    chr.MoonTime -= time.ElaspedMsDelta;
                    if (chr.MoonTime < 0)
                        chr.MoonTime = 0;
                }
            }
            
        }

        bool CanHpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Sick))
                return false;
            if (HasConditionEffect(ConditionEffects.Bleeding))
                return false;
            return true;
        }

        bool CanMpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Quiet) ||
                HasConditionEffect(ConditionEffects.NinjaSpeedy))
                return false;
            
            return true;
        }

        internal void SetNewbiePeriod()
        {
            _newbieTime = 3000;
        }

        internal void SetTPDisabledPeriod()
        {
            _canTpCooldownTime = 5 * 1000; // 10 seconds
        }

        public bool IsVisibleToEnemy()
        {
            if (HasConditionEffect(ConditionEffects.Paused))
                return false;
            if (HasConditionEffect(ConditionEffects.Invisible))
                return false;
            if (HasConditionEffect(ConditionEffects.Hidden))
                return false;
            if (_newbieTime > 0)
                return false;
            return true;
        }

        public bool TPCooledDown()
        {
            if (_canTpCooldownTime > 0)
                return false;
            return true;
        }

        public bool DecoyCooledDown()
        {
            if (_canDecoyCooldownTime > 0)
                return false;
            return true;
        }

        public bool CanEnterMoon()
        {
            var chr = _client.Character;
            if (chr.MoonTime > 0)
                return false;
            return true;
        }
    }
}
