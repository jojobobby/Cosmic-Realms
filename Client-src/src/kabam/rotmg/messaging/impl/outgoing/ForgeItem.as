package kabam.rotmg.messaging.impl.outgoing
{
    import flash.utils.IDataOutput;

    public class ForgeItem extends OutgoingMessage
    {

        public var targetSlotId:int;
        public var newName:String;
        public var inputItemSlotId:int;
        public var modifiers:Array;
        public var isPresent:Boolean;
        public var newDescription:String;

        public function ForgeItem(_arg1:uint, _arg2:Function)
        {
            super(_arg1, _arg2);
        }

        override public function writeToOutput(_arg1:IDataOutput):void
        {
            var _local_2:int;
            _arg1.writeUTF(this.newName);
            _arg1.writeByte(this.targetSlotId);
            _arg1.writeByte(this.inputItemSlotId);
            _arg1.writeByte(this.modifiers.length);
            for each (_local_2 in this.modifiers)
            {
                _arg1.writeByte(_local_2);
            }
            _arg1.writeBoolean(this.isPresent);
            _arg1.writeUTF(((this.newDescription) || ("")));
        }

        override public function toString():String {
            return formatToString("FORGE_ITEM", "newName_", "targetSlotId_", "inputItemSlotId_", "modifiers_", "isPresent_", "newDescription_");
        }
    }
}