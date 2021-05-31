package com.company.assembleegameclient.objects
{
    import com.company.assembleegameclient.engine3d.Point3D;
    import com.company.assembleegameclient.map.Camera;
    import com.company.assembleegameclient.map.Map;
    import com.company.assembleegameclient.map.Square;
    import com.company.assembleegameclient.objects.particles.HitEffect;
    import com.company.assembleegameclient.objects.particles.SparkParticle;
    import com.company.assembleegameclient.parameters.Parameters;
    import com.company.assembleegameclient.tutorial.Tutorial;
    import com.company.assembleegameclient.tutorial.doneAction;
    import com.company.assembleegameclient.util.BloodComposition;
    import com.company.assembleegameclient.util.FreeList;
    import com.company.assembleegameclient.util.RandomUtil;
    import com.company.assembleegameclient.util.TextureRedrawer;
    import com.company.util.GraphicsUtil;
    import com.company.util.Trig;
import com.hurlant.util.asn1.parser.boolean;

import flash.display.BitmapData;
    import flash.display.GraphicsGradientFill;
    import flash.display.GraphicsPath;
    import flash.display.IGraphicsData;
    import flash.geom.Matrix;
    import flash.geom.Point;
    import flash.geom.Vector3D;
    import flash.utils.Dictionary;

    public class Projectile extends BasicObject
    {
        private static var objBullIdToObjId_:Dictionary = new Dictionary();
        public var props_:ObjectProperties;
        public var containerProps_:ObjectProperties;
        public var projProps_:ProjectileProperties;
        public var texture_:BitmapData;
        public var bulletId_:uint;
        public var ownerId_:int;
        public var containerType_:int;
        public var bulletType_:uint;
        public var damagesEnemies_:Boolean;
        public var damagesPlayers_:Boolean;
        public var damage_:int;
        public var sound_:String;
        public var startX_:Number;
        public var startY_:Number;
        public var startTime_:int;
        public var angle_:Number = 0;
        public var multiHitDict_:Dictionary;
        public var p_:Point3D = new Point3D(100);
        private var staticPoint_:Point = new Point();
        private var staticVector3D_:Vector3D = new Vector3D();
        protected var shadowGradientFill_:GraphicsGradientFill = new GraphicsGradientFill("radial", [0, 0], [0.5, 0], null, new Matrix());
        protected var shadowPath_:GraphicsPath = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
        private var size:int;

        public function Projectile()
        {
            super();
        }

        public static function findObjId(_arg1:int, _arg2:uint) : int
        {
            return objBullIdToObjId_[_arg2 << 24 | _arg1];
        }

        public static function getNewObjId(_arg1:int, _arg2:uint):int
        {
            var _local3:int = getNextFakeObjectId();
            objBullIdToObjId_[_arg2 << 24 | _arg1] = _local3;
            return _local3;
        }

        public static function removeObjId(_arg1:int, _arg2:uint):void
        {
            delete objBullIdToObjId_[_arg2 << 24 | _arg1];
        }

        public static function dispose():void
        {
            objBullIdToObjId_ = new Dictionary();
        }


        public function reset(_arg1:int, _arg2:int, _arg3:int, _arg4:int, _arg5:Number, _arg6:int, _arg7:String = "", _arg8:String = ""):void {
            var _local11:Number;
            clear();
            this.containerType_ = _arg1;
            this.bulletType_ = _arg2;
            this.ownerId_ = _arg3;
            this.bulletId_ = _arg4;
            this.angle_ = Trig.boundToPI(_arg5);
            this.startTime_ = _arg6;
            objectId_ = getNewObjId(this.ownerId_, this.bulletId_);
            z_ = 0.5;
            this.containerProps_ = ObjectLibrary.propsLibrary_[this.containerType_];
            this.projProps_ = this.containerProps_.projectiles_[_arg2];
            var _local9:String = ((((!((_arg7 == ""))) && ((this.projProps_.objectId_ == _arg8)))) ? _arg7 : this.projProps_.objectId_);
            this.props_ = ObjectLibrary.getPropsFromId(_local9);
            hasShadow_ = (this.props_.shadowSize_ > 0);
            var _local10:TextureData = ObjectLibrary.typeToTextureData_[this.props_.type_];
            this.texture_ = _local10.getTexture(objectId_);
            this.damagesPlayers_ = this.containerProps_.isEnemy_;
            this.damagesEnemies_ = !(this.damagesPlayers_);
            this.sound_ = this.containerProps_.oldSound_;
            this.multiHitDict_ = ((this.projProps_.multiHit_) ? new Dictionary() : null);
            if(this.projProps_.size_ > 0)
            {
                _local11 = this.projProps_.size_;
            }
            else
            {
                _local11 = ObjectLibrary.getSizeFromType(this.containerType_);
            }
            var _loc12_:Number = this.texture_.width / 8;
            this.p_.setSize((8 * (_local11 / 100)));
            this.size = _local11 / _loc12_;
            this.damage_ = 0;
        }

        public var _isCrit:Number = 1;

        public function setDamage(_arg1:int, _arg2:Number = 1.0):void {
            this._isCrit = _arg2;
            this.damage_ = _arg1;
        }

        override public function addTo(_arg1:Map, _arg2:Number, _arg3:Number):Boolean {
            var _local4:Player;
            this.startX_ = _arg2;
            this.startY_ = _arg3;
            if (!super.addTo(_arg1, _arg2, _arg3)) {
                return false;
            }
            if (((!(this.containerProps_.flying_)) && (square_.sink_))) {
                z_ = 0.1;
            }
            else {
                _local4 = (_arg1.goDict_[this.ownerId_] as Player);
                if (((!((_local4 == null))) && ((_local4.sinkLevel_ > 0)))) {
                    z_ = (0.5 - (0.4 * (_local4.sinkLevel_ / Parameters.MAX_SINK_LEVEL)));
                }
            }
            return true;
        }

        public function moveTo(_arg1:Number, _arg2:Number):Boolean {
            var _local3:Square = map_.getSquare(_arg1, _arg2);
            if (_local3 == null) {
                return false;
            }
            x_ = _arg1;
            y_ = _arg2;
            square_ = _local3;
            return true;
        }

        override public function removeFromMap():void {
            super.removeFromMap();
            removeObjId(this.ownerId_, this.bulletId_);
            this.multiHitDict_ = null;
            FreeList.deleteObject(this);
        }

        private function positionAt(_arg1:int, _arg2:Point):void {
            var _local5:Number;
            var _local6:Number;
            var _local7:Number;
            var _local8:Number;
            var _local9:Number;
            var _local10:Number;
            var _local11:Number;
            var _local12:Number;
            var _local13:Number;
            var _local14:Number;
            _arg2.x = this.startX_;
            _arg2.y = this.startY_;
            var _local3:Number = (_arg1 * (this.projProps_.speed_ / 10000));
            var _local4:Number = ((((this.bulletId_ % 2)) == 0) ? 0 : Math.PI);
            if (this.projProps_.wavy_) {
                _local5 = (6 * Math.PI);
                _local6 = (Math.PI / 64);
                _local7 = (this.angle_ + (_local6 * Math.sin((_local4 + ((_local5 * _arg1) / 1000)))));
                _arg2.x = (_arg2.x + (_local3 * Math.cos(_local7)));
                _arg2.y = (_arg2.y + (_local3 * Math.sin(_local7)));
            }
            else {
                if (this.projProps_.parametric_) {
                    _local8 = (((_arg1 / this.projProps_.lifetime_) * 2) * Math.PI);
                    _local9 = (Math.sin(_local8) * (((this.bulletId_ % 2)) ? 1 : -1));
                    _local10 = (Math.sin((2 * _local8)) * ((((this.bulletId_ % 4)) < 2) ? 1 : -1));
                    _local11 = Math.sin(this.angle_);
                    _local12 = Math.cos(this.angle_);
                    _arg2.x = (_arg2.x + (((_local9 * _local12) - (_local10 * _local11)) * this.projProps_.magnitude_));
                    _arg2.y = (_arg2.y + (((_local9 * _local11) + (_local10 * _local12)) * this.projProps_.magnitude_));
                }
                else {
                    if (this.projProps_.boomerang_) {
                        _local13 = ((this.projProps_.lifetime_ * (this.projProps_.speed_ / 10000)) / 2);
                        if (_local3 > _local13) {
                            _local3 = (_local13 - (_local3 - _local13));
                        }
                    }
                    else if(this.projProps_.blazingBoomerang_){
                        _local13 = ((this.projProps_.lifetime_ * (this.projProps_.speed_ / 10000)) / 1.25);
                        if (_local3 > _local13) {
                            _local3 = (_local13 - (_local3 - _local13));
                        }
                        this.angle_ += _local3 / 500;
                    }
                    else if(this.projProps_.vargoSpellBoomerang_){
                        var xVal:Number = ((this.projProps_.lifetime_ * (this.projProps_.speed_ / 10000)) / 4);
                        if(_local3 > 2 * xVal)
                            _local3 -=  2 * xVal;
                        else if(_local3 > xVal)
                            _local3 = xVal - (_local3 - xVal);
                        this.angle_ += _local3 / 5;
                    }
                        _arg2.x = (_arg2.x + (_local3 * Math.cos(this.angle_)));
                        _arg2.y = (_arg2.y + (_local3 * Math.sin(this.angle_)));
                    if (this.projProps_.amplitude_ != 0) {
                        _local14 = (this.projProps_.amplitude_ * Math.sin((_local4 + ((((_arg1 / this.projProps_.lifetime_) * this.projProps_.frequency_) * 2) * Math.PI))));
                        _arg2.x = (_arg2.x + (_local14 * Math.cos((this.angle_ + (Math.PI / 2)))));
                        _arg2.y = (_arg2.y + (_local14 * Math.sin((this.angle_ + (Math.PI / 2)))));
                    }
                }
            }
        }

        override public function update(currentTime:int, msDelta:int):Boolean {
            var blood:Vector.<uint>;

            var lifetime:int = currentTime - this.startTime_;
            if (lifetime > this.projProps_.lifetime_) {
                return false;
            }

            var pnt:Point = this.staticPoint_;
            this.positionAt(lifetime, pnt);

            if (!this.moveTo(pnt.x, pnt.y) || square_.tileType_ == 0xFFFF) {
                if (this.damagesPlayers_) {
                    map_.gs_.gsc_.squareHit(currentTime, this.bulletId_, this.ownerId_);
                }
                else {
                    if (square_.obj_ != null) {
                        blood = BloodComposition.getColors(this.texture_);
                        map_.addObj(new HitEffect(blood, 100, 3, this.angle_, this.projProps_.speed_), pnt.x, pnt.y);
                    }
                }
                return false;
            }

            if (square_.obj_ != null &&
                    (!square_.obj_.props_.isEnemy_ || !this.damagesEnemies_) &&
                    (square_.obj_.props_.enemyOccupySquare_ || !this.projProps_.passesCover_ && square_.obj_.props_.occupySquare_)) {
                if (this.damagesPlayers_) {
                    map_.gs_.gsc_.otherHit(currentTime, this.bulletId_, this.ownerId_, square_.obj_.objectId_);
                }
                else {
                    blood = BloodComposition.getColors(this.texture_);
                    map_.addObj(new HitEffect(blood, 100, 3, this.angle_, this.projProps_.speed_), pnt.x, pnt.y);
                }
                return false;
            }

            var go:GameObject = this.getHit(pnt.x, pnt.y);
            if (go != null) {
                var player:Player = map_.player_;
                var goIsEnemy:Boolean = go.props_.isEnemy_;
                var goHit:Boolean = player != null &&
                        !player.isPaused() &&
                        !player.isHidden() &&
                        (this.damagesPlayers_ || goIsEnemy && this.ownerId_ == player.objectId_);

                if (goHit) {
                    var dmg:int = GameObject.damageWithDefense(this.damage_, go.defense_, this.projProps_.armorPiercing_, go.condition_);

                    var killed:Boolean = false;
                    if (go.hp_ <= dmg) {
                        killed = true;
                        if (goIsEnemy) {
                            doneAction(map_.gs_, Tutorial.KILL_ACTION);
                        }
                    }

                    if (go == player) {
                        map_.gs_.gsc_.playerHit(this.bulletId_, this.ownerId_);
                        if (containerType_ != 0x1392)
                        {
                            go.damage(this.containerType_, dmg, null, false, this); //null = this.projProps_.effects_
                        }
                    }

                    else {
                        if (goIsEnemy) {
                            map_.gs_.gsc_.enemyHit(currentTime, this.bulletId_, go.objectId_, killed);
                            go.damage(this.containerType_, dmg, this.projProps_.effects_, killed, this);
                            if(go != null && (go.props_.isQuest_))
                             {
                                 if(isNaN(Parameters.DamageCounter[go.objectId_])){
                                     Parameters.DamageCounter[go.objectId_] = 0;
                                 }
                                 var targetId:* = go.objectId_;
                                 var damage:* = Parameters.DamageCounter[targetId] + dmg;
                                 Parameters.DamageCounter[targetId] = damage;
                             }

                        }
                        else {
                            if (!this.projProps_.multiHit_) {
                                map_.gs_.gsc_.otherHit(currentTime, this.bulletId_, this.ownerId_, go.objectId_);
                            }
                        }
                    }
                }

                if (this.projProps_.multiHit_) {
                    this.multiHitDict_[go] = true;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        public function getHit(pX:Number, pY:Number):GameObject {
            var go:GameObject = null;
            var xDiff:Number = NaN;
            var yDiff:Number = NaN;
            var dist:Number = NaN;
            var minDist:Number = Number.MAX_VALUE;
            var minGO:GameObject = null;

            for each(go in map_.goDict_)
            {
                if(!go.isInvincible())
                {
                    if(!go.isStasis())
                    {
                        if(this.damagesEnemies_ && go.props_.isEnemy_ || this.damagesPlayers_ && go.props_.isPlayer_)
                        {
                            if(!(go.dead_ || go.isPaused()))
                            {
                                xDiff = go.x_ > pX?Number(go.x_ - pX):Number(pX - go.x_);
                                yDiff = go.y_ > pY?Number(go.y_ - pY):Number(pY - go.y_);
                                if(!(xDiff > go.radius_ || yDiff > go.radius_))
                                {
                                    if(!(this.projProps_.multiHit_ && this.multiHitDict_[go] != null))
                                    {
                                        if(go == map_.player_)
                                        {
                                            return go;
                                        }
                                        dist = Math.sqrt(xDiff * xDiff + yDiff * yDiff);
                                        if(dist < minDist)
                                        {
                                            minDist = dist;
                                            minGO = go;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return minGO;
        }

        override public function draw(param1:Vector.<IGraphicsData>, param2:Camera, param3:int) : void
        {
            var _loc4_:BitmapData = this.texture_;
            if (Parameters.data_.projOutline) {
                _loc4_ = TextureRedrawer.redraw(_loc4_,this.size, false, props_.GlowColor_, true, 5, 1.4, 3);
            }
            var _loc5_:Number = this.props_.rotation_ == 0?Number(0):Number(param3 / this.props_.rotation_);
            this.staticVector3D_.x = x_;
            this.staticVector3D_.y = y_;
            this.staticVector3D_.z = z_;
            var _loc6_:Number = !Parameters.data_.smartProjectiles?Number(this.angle_):Number(this.getDirectionAngle(param3));
            var _loc7_:Number = _loc6_ - param2.angleRad_ + this.props_.angleCorrection_ + _loc5_;
            this.p_.draw(param1,this.staticVector3D_,_loc7_,param2.wToS_,param2,_loc4_);
            if(this.projProps_.particleTrail_)
            {
                if(Parameters.data_.eyeCandyParticles)
                {
                    map_.addObj(new SparkParticle(100,16711935,600,0.5,RandomUtil.plusMinus(3),RandomUtil.plusMinus(3)),x_,y_);
                    map_.addObj(new SparkParticle(100,16711935,600,0.5,RandomUtil.plusMinus(3),RandomUtil.plusMinus(3)),x_,y_);
                    map_.addObj(new SparkParticle(100,16711935,600,0.5,RandomUtil.plusMinus(3),RandomUtil.plusMinus(3)),x_,y_);
                }
            }
        }

        private function getDirectionAngle(param1:int) : Number
        {
            var _loc2_:int = param1 - this.startTime_;
            var _loc3_:Point = new Point();
            this.positionAt(_loc2_ + 16,_loc3_);
            var _loc4_:Number = _loc3_.x - x_;
            var _loc5_:Number = _loc3_.y - y_;
            return Math.atan2(_loc5_,_loc4_);
        }


        override public function drawShadow(_arg1:Vector.<IGraphicsData>, _arg2:Camera, _arg3:int):void {
            if (!Parameters.drawProj_) {
                return;
            }
            var _local4:Number = (this.props_.shadowSize_ / 400);
            var _local5:Number = (30 * _local4);
            var _local6:Number = (15 * _local4);
            this.shadowGradientFill_.matrix.createGradientBox((_local5 * 2), (_local6 * 2), 0, (posS_[0] - _local5), (posS_[1] - _local6));
            _arg1.push(this.shadowGradientFill_);
            this.shadowPath_.data.length = 0;
            Vector.<Number>(this.shadowPath_.data).push((posS_[0] - _local5), (posS_[1] - _local6), (posS_[0] + _local5), (posS_[1] - _local6), (posS_[0] + _local5), (posS_[1] + _local6), (posS_[0] - _local5), (posS_[1] + _local6));
            _arg1.push(this.shadowPath_);
        }
    }
}