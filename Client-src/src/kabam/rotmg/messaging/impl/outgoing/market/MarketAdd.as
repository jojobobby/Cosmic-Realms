package kabam.rotmg.messaging.impl.outgoing.market
{

import flash.utils.IDataOutput;

import kabam.rotmg.messaging.impl.outgoing.OutgoingMessage;

public class MarketAdd extends OutgoingMessage
{
    public var slot:int;
    public var price_:int;

    public function MarketAdd(id:uint, callback:Function)
    {
        super(id,callback);
    }

    override public function writeToOutput(data:IDataOutput) : void
    {
        data.writeByte(this.slot);
        data.writeInt(this.price_);
    }
}
}
