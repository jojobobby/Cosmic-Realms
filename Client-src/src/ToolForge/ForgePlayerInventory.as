package ToolForge
{
    import com.company.assembleegameclient.ui.panels.itemgrids.EquippedGrid;
    import com.company.assembleegameclient.ui.panels.itemgrids.InventoryGrid;
    import flash.display.Sprite;
    import com.company.assembleegameclient.objects.Player;
    import flash.events.Event;
    import kabam.rotmg.text.view.TextFieldDisplayConcrete;
    import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

    public class ForgePlayerInventory extends Sprite
    {
        private var title:TextFieldDisplayConcrete;
        private var equipmentInventory:EquippedGrid;
        private var itemInventory:InventoryGrid;
        private var backpackInventory:InventoryGrid;
        private var player:Player;

        public function ForgePlayerInventory(player:Player)
        {
            this.player = player;
            this.title = new TextFieldDisplayConcrete().setSize(21).setBold(true).setColor(0xFFFFFF).setStringBuilder(new StaticStringBuilder("Inventory"));
            addChild(this.title);
            this.equipmentInventory = new EquippedGrid(player, player.slotTypes_, player);
            this.itemInventory = new InventoryGrid(player, player, 4);
            if (player.hasBackpack_)
            {
                this.backpackInventory = new InventoryGrid(player, player, 12);
            }
            this.equipmentInventory.y = 35;
            this.itemInventory.y = 85;
            this.itemInventory.x = 0;
            if (player.hasBackpack_)
            {
                this.backpackInventory.y = 85;
                this.backpackInventory.x = 180;
            }
            addChild(this.equipmentInventory);
            addChild(this.itemInventory);
            if (player.hasBackpack_)
            {
                addChild(this.backpackInventory);
            }
            addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        }

        private function onEnterFrame(event:Event):void
        {
            this.equipmentInventory.setItems(this.player.equipment_);
            this.itemInventory.setItems(this.player.equipment_);
            if (this.player.hasBackpack_)
            {
          //      this.backpackInventory.setItems(this.player.equipment_);
            }
        }
    }
}