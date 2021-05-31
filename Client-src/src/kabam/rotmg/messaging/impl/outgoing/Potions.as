package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class Potions extends OutgoingMessage {

    public var type_:int;
    public var max_:Boolean;

    public function Potions(_arg1:uint, _arg2:Function) {
        super (_arg1, _arg2)
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.type_);
        _arg1.writeBoolean(this.max_);
    }

    override public function toString():String {
        return (formatToString("POTIONS", "type_", "max_"));
}


}
}
