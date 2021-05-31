package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class SwordDmg extends IncomingMessage {

    public var DmgMultiplied_:Number;
    public var ActivatedSword_:Boolean;


    public function SwordDmg(_arg_1:uint, _arg_2:Function) {
        super(_arg_1, _arg_2);
    }

    override public function parseFromInput(_arg_1:IDataInput):void {

        this.DmgMultiplied_ = _arg_1.readFloat();
        this.ActivatedSword_ = _arg_1.readBoolean();
    }

    override public function toString():String {
        return (formatToString("CRITICALDAMAGE", "DmgMultiplied_", "ActivatedSword_"));
    }


}
}//package kabam.rotmg.messaging.impl.incoming
