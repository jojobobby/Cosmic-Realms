using common.resources;
using log4net;

namespace wServer.realm.entities
{
    partial class Player
    {
        private static readonly ILog CheatLog = LogManager.GetLogger("CheatLog");

        private const int _freqThreshold = 40;
        private const int _violationThreshold = 10;

        private int _preTime = 0;
        private int _freqViolation = 0;
        private int _nonViolationPer = 0;
        private ushort _curType = 255;

        private int _periodAt = 1;
        private int _curPeriod = 0;
        public bool ValidatePlayerShoot(int time, Item item) 
        {
            if (_curType != item.ObjectType)
            {
                _curType = item.ObjectType;
                _periodAt = item.NumProjectiles;
                _curPeriod = 0;
            }

            if (_preTime == 0)
                _preTime = time;

            _curPeriod++;

            if (_curPeriod < _periodAt) 
                return true;
            else 
                _curPeriod = 0;

            var difference = time - _preTime;
            if (Stats.GetAttackFreq() - difference > _freqThreshold)
                _freqViolation++;
            else
                _nonViolationPer++;

            if (_nonViolationPer > _violationThreshold)
            {
                _freqViolation = 0;
                _nonViolationPer = 0;
            }

            if (_freqViolation > _violationThreshold)
            {
                _freqViolation = 0;
                _nonViolationPer = 0;

                return false;
            }    

            _preTime = time;
            return true;
        }

        public bool IsNoClipping()
        {
            if (Owner == null || !TileOccupied(RealX, RealY) && !TileFullOccupied(RealX, RealY))
                return false;

            CheatLog.Info($"{Name} is walking on an occupied tile.");
            return true;
        }
    }
}