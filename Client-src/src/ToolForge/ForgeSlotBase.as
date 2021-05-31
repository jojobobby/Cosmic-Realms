package ToolForge
{
    import flash.display.Sprite;
    import flash.display.Shape;
    import flash.display.Bitmap;
    import flash.events.Event;
    import kabam.rotmg.pets.util.PetsViewAssetFactory;
    import kabam.rotmg.text.view.TextFieldDisplayConcrete;
    import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
    import kabam.rotmg.text.view.stringBuilder.LineBuilder;

    class ForgeSlotBase extends Sprite
    {
        public var itemId:int = -1;
        public var slotId:int = -1;
        public var objectId:int = -1;
        protected var bg:Shape;
        protected var itemSprite:Sprite;
        protected var slotIcon:Bitmap;
        private var titleText:TextFieldDisplayConcrete;
        private var titleStringBuilder:LineBuilder;
        private var descriptionText:TextFieldDisplayConcrete;
        private var descriptionLineBuilder:AppendingLineBuilder;

        public function ForgeSlotBase()
        {
            this.bg = PetsViewAssetFactory.returnPetSlotShape(46, 0x545454, 0, true, false);
            this.titleText = PetsViewAssetFactory.returnPetSlotTitle();
            this.titleStringBuilder = new LineBuilder();
            this.descriptionText = PetsViewAssetFactory.returnMediumCenteredTextfield(16777103, 100);
            this.descriptionLineBuilder = new AppendingLineBuilder();
            this.itemSprite = new Sprite();
            this.slotIcon = new Bitmap();
            super();
            this.addElements();
            this.descriptionText.textChanged.add(this.onTextChanged);
            this.titleText.textChanged.add(this.onTextChanged);
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }

        public function setTitle(s:String, object:Object):void
        {
            this.titleStringBuilder.setParams(s, object);
            this.titleText.setStringBuilder(this.titleStringBuilder);
        }

        public function setDescription(s:String, object:Object):void
        {
            this.descriptionLineBuilder.clear();
            this.descriptionLineBuilder.pushParams(s, object);
            this.descriptionText.setStringBuilder(this.descriptionLineBuilder);
        }

        protected function positionIcon():void
        {
            this.itemSprite.x = 0;
            this.itemSprite.y = 0;
            this.slotIcon.x = ((100 - this.slotIcon.width) * 0.5);
            this.slotIcon.y = ((46 - this.slotIcon.height) * 0.5);
        }

        private function addElements():void
        {
            this.itemSprite.addChild(this.slotIcon);
            addChild(this.bg);
            addChild(this.titleText);
            addChild(this.descriptionText);
            addChild(this.itemSprite);
        }

        private function onTextChanged():void
        {
            this.descriptionText.y = ((this.titleText.y + this.titleText.height) - 1);
        }

        protected function onRemovedFromStage(_arg_1:Event):void
        {
            removeEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
            this.descriptionText.textChanged.remove(this.onTextChanged);
            this.titleText.textChanged.remove(this.onTextChanged);
        }
    }
}