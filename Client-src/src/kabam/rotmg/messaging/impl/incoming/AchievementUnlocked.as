package kabam.rotmg.messaging.impl.incoming
{
    import flash.utils.IDataInput;

    public class AchievementUnlocked extends IncomingMessage
    {
        public var title_:String;
        public var description_:String;
        public var iconId_:int;

        public function AchievementUnlocked(_arg1:uint, _arg2:Function)
        {
            super(_arg1, _arg2);
        }

        override public function parseFromInput(_arg1:IDataInput):void
        {
            this.title_ = _arg1.readUTF();
            this.description_ = _arg1.readUTF();
            this.iconId_ = _arg1.readInt();
        }

        override public function toString():String
        {
            return formatToString("ACHIEVEMENT_UNLOCKED", "title_", "description_", "iconId_");
        }
    }
}