package ToolForge.SignalCommands
{
    import ToolForge.ForgeItemInformation;
    import com.company.assembleegameclient.game.AGameSprite;
    import kabam.rotmg.core.model.PlayerModel;

    public class ForgeItemCommand
    {
        [Inject]
        public var info:ForgeItemInformation;
        [Inject]
        public var gs:AGameSprite;
        [Inject]
        public var model:PlayerModel;

        public function execute():void
        {
            this.gs.gsc_.forgeItem(this.info.inputItemSlotId, this.info.targetSlotId, this.info.newName, this.info.modifiers, this.info.isPresent, this.info.newDescription);
            //Sfx.play("forge");
        }
    }
}