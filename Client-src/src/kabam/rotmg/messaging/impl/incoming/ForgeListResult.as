package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class ForgeListResult extends IncomingMessage {
    public var Result:String;
    public var Recipes:Vector.<String>;

    public function ForgeListResult(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    public override function parseFromInput(input:IDataInput):void {
        Recipes = new Vector.<String>();
        this.Result = input.readUTF();

        var ln:int = input.readInt();
        for (var i:int = 0; i < ln; i++) {
            Recipes.push(input.readUTF());
        }
    }
}
}
