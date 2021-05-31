package ToolForge.Mediators
{
    import ToolForge.ToolForgeFrame;
    import ToolForge.SignalCommands.ForgeItemSignal;
    import ToolForge.ForgeLock;
    import com.company.assembleegameclient.game.AGameSprite;
    import flash.events.Event;
    import ToolForge.ForgeItemInformation;
    import kabam.rotmg.dialogs.control.CloseDialogsSignal;
    import robotlegs.bender.bundles.mvcs.Mediator;

    public class ToolForgeMediator extends Mediator
    {

        [Inject]
        public var closeDialogs:CloseDialogsSignal;
        [Inject]
        public var view:ToolForgeFrame;
        [Inject]
        public var forgeItem:ForgeItemSignal;
        [Inject]
        public var forgeLock:ForgeLock;

        override public function initialize():void
        {
            this.forgeLock.isOpen = true;
            this.view.close.add(this.onClose);
            this.view.forgeItem.add(this.onForgeItem);
            this.view.addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }

        override public function destroy():void
        {
            this.forgeLock.isOpen = true;
            this.view.close.remove(this.onClose);
            this.view.forgeItem.remove(this.onForgeItem);
            this.view.removeEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
            super.destroy();
        }

        private function onClose():void
        {
            this.closeDialogs.dispatch();
        }

        private function onForgeItem(aGameSprite:AGameSprite, information:ForgeItemInformation):void
        {
            this.forgeItem.dispatch(aGameSprite, information);
        }

        private function onRemovedFromStage(event:Event):void
        {
            this.forgeLock.isOpen = false;
            this.view.removeEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }
    }
}