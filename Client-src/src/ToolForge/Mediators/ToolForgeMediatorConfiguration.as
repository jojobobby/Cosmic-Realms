package ToolForge.Mediators
{
    import ToolForge.ForgeLock;
    import ToolForge.ToolForgePanel;
    import ToolForge.ToolForgeFrame;
    import ToolForge.ForgeInventorySlot;
    import ToolForge.ForgeModifierButton;
    import ToolForge.SignalCommands.ForgeItemSignal;
    import ToolForge.SignalCommands.ForgeItemCommand;
import ToolForge.forgeList.ForgeListItem;

import org.swiftsuspenders.Injector;
    import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
    import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;
    import robotlegs.bender.framework.api.IConfig;

    public class ToolForgeMediatorConfiguration implements IConfig
    {
        [Inject]
        public var injector:Injector;
        [Inject]
        public var mediatorMap:IMediatorMap;
        [Inject]
        public var signalCommandMap:ISignalCommandMap;

        public function configure():void
        {
            this.injector.map(ForgeLock).asSingleton();
            this.mediatorMap.map(ToolForgePanel).toMediator(ToolForgePanelMediator);
            this.mediatorMap.map(ToolForgeFrame).toMediator(ToolForgeMediator);
            this.mediatorMap.map(ForgeInventorySlot).toMediator(ForgeInventorySlotMediator);
            this.mediatorMap.map(ForgeModifierButton).toMediator(ForgeModifierButtonMediator);
            this.mediatorMap.map(ForgeListItem).toMediator(ForgeListItemMediator);
            this.mapSignalCommands();
        }

        private function mapSignalCommands():void
        {
            this.signalCommandMap.map(ForgeItemSignal).toCommand(ForgeItemCommand);
        }
    }
}