﻿package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.sound.SoundEffectLibrary;
    import com.company.assembleegameclient.map.Camera;

    import flash.display.IGraphicsData;

    import kabam.rotmg.fortune.services.FortuneModel;

public class Avatar extends GameObject {

    public var hurtSound_:String;
    public var deathSound_:String;

    public function Avatar(_arg1:XML) {
        super(_arg1);
        this.hurtSound_ = ((_arg1.hasOwnProperty("HitSound")) ? String(_arg1.HitSound) : "monster/default_hit");
        SoundEffectLibrary.load(this.hurtSound_);
        this.deathSound_ = ((_arg1.hasOwnProperty("DeathSound")) ? String(_arg1.DeathSound) : "monster/default_death");
        SoundEffectLibrary.load(this.deathSound_);
    }
    override public function draw(_arg1:Vector.<IGraphicsData>, _arg2:Camera, _arg3:int):void {

            super.draw(_arg1, _arg2, _arg3);

    }
    override public function damage(_arg1:int, _arg2:int, _arg3:Vector.<uint>, _arg4:Boolean, _arg5:Projectile):void {
        super.damage(_arg1, _arg2, _arg3, _arg4, _arg5);
        if (dead_) {
            SoundEffectLibrary.play(this.deathSound_);
        }
        else {
            if (((_arg5) || ((_arg2 > 0)))) {
                SoundEffectLibrary.play(this.hurtSound_);
            }
        }
    }


}
}
