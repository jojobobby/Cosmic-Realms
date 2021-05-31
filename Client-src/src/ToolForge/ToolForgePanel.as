package ToolForge
{

    import com.company.assembleegameclient.ui.panels.ButtonPanel;
    import flash.events.Event;
    import com.company.assembleegameclient.game.GameSprite;
    import flash.events.MouseEvent;
    import flash.events.KeyboardEvent;
    import com.company.assembleegameclient.parameters.Parameters;
    import kabam.rotmg.text.model.TextKey;
    import org.osflash.signals.Signal;

    public class ToolForgePanel extends ButtonPanel
    {
        public var openFrame:Signal;

        public function ToolForgePanel(gameSprite:GameSprite)
        {
            super(gameSprite, "Forge Station", TextKey.PANEL_VIEW_BUTTON);
            this.openFrame = new Signal();
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }

        override protected function onButtonClick(event:MouseEvent):void
        {
            this.internalOpen();
        }

        private function internalOpen():void
        {
            this.openFrame.dispatch();
        }

        private function onAddedToStage(event:Event):void
        {
            stage.addEventListener(KeyboardEvent.KEY_DOWN, this.onKeyDown);
            removeEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        }

        private function onKeyDown(event:KeyboardEvent):void
        {
            if (event.keyCode == Parameters.data_.interact)
            {
                this.internalOpen();
            }
        }

        private function onRemovedFromStage(event:Event):void
        {
            stage.removeEventListener(KeyboardEvent.KEY_DOWN, this.onKeyDown);
            removeEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }
    }
}