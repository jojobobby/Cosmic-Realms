
using System.Linq;
using wServer.realm;
using wServer.realm.entities;
using System;
using System.Collections.Generic;
using wServer.realm;
using wServer.realm.entities;



namespace wServer.logic.behaviors
{
    internal class ScaleHP2 : Behavior
    {
        private readonly int _percentage;
        private readonly double _range;
        private readonly int _scaleAfter;

        class ScaleHPState
        {
            public IList<string> pNamesCounted;
            public int initialScaleAmount = 0;
            public int cooldown;
        }

        public ScaleHP2(int amount, int scaleStart = 0, double range = 25.0)
        {
            _percentage = amount;
            _range = range;
            _scaleAfter = 1;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = new ScaleHPState
            {
                pNamesCounted = new List<string>(),
                initialScaleAmount = _scaleAfter,
                cooldown = 0
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            ScaleHPState scstate = (ScaleHPState)state;
            if (scstate.cooldown <= 0)
            {
                scstate.cooldown = 1000;
                if (!(host is Enemy)) return;
                Enemy enemy = host as Enemy;

                int plrCount = 0;
                foreach (var i in host.Owner.Players)
                {
                    if (i.Value.Client.Character.Fame < 100) continue;
                    if (scstate.pNamesCounted.Contains(i.Value.Name)) continue;
                    if (_range > 0)
                    {
                        if (host.Dist(i.Value) < _range)
                          scstate.pNamesCounted.Add(i.Value.Name);
                    }
                    else
                       scstate.pNamesCounted.Add(i.Value.Name);
                    
                   
                }
                plrCount = scstate.pNamesCounted.Count;

                if (plrCount > scstate.initialScaleAmount)
                {
                    var amountPerPlayer = _percentage * enemy.ObjectDesc.MaxHP / 100;
                    int amountInc = (plrCount - scstate.initialScaleAmount) * amountPerPlayer;
                    scstate.initialScaleAmount += (plrCount - scstate.initialScaleAmount);

                    int newHpMaximum = enemy.MaximumHP + amountInc;

                    enemy.HP += amountInc;
                    enemy.MaximumHP = newHpMaximum;
                }
            }
            else
                scstate.cooldown -= time.ElaspedMsDelta;

            state = scstate;
        }
    }
}