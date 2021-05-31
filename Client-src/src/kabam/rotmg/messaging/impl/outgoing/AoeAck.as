package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

import kabam.rotmg.messaging.impl.data.WorldPosData;

public class AoeAck extends OutgoingMessage {

    public var time_:int;
    public var position_:WorldPosData;
    public var damage_:int;
    public var objectName_:String;
    public var effect_:int;
    public var duration_:Number;

    public function AoeAck(_arg1:uint, _arg2:Function) {
        this.position_ = new WorldPosData();
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.time_);
        this.position_.writeToOutput(_arg1);
        _arg1.writeInt(this.damage_);
        _arg1.writeUTF(this.objectName_);
        _arg1.writeByte(this.effect_);
        _arg1.writeFloat(this.duration_);
    }

    override public function toString():String {
        return (formatToString("AOEACK", "time_", "position_", "damage_", "objectName_", "effect_", "duration_"));
    }


}
}
