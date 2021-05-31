package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class Upgrade extends OutgoingMessage {

    public var lunar:Boolean;
    public var earth:Boolean;
    public var enhancers:Boolean;

    public function Upgrade(_arg1:uint, _arg2:Function) {
        super (_arg1, _arg2)
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeBoolean(this.lunar);
        _arg1.writeBoolean(this.earth);
        _arg1.writeBoolean(this.enhancers);
    }

    override public function toString():String {
        return (formatToString("UPGRADE", "lunar", "earth", "enhancers"));
    }


}
}
