package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class ForgeList extends OutgoingMessage{

    public var Category:int;

    public function ForgeList(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    public override function writeToOutput(writer:IDataOutput):void {
        writer.writeInt(Category);
    }

    override public function toString():String {
        return (formatToString("FORGE_LIST"));
    }
}
}
