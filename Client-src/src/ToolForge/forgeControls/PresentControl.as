package ToolForge.forgeControls
{
    import com.company.assembleegameclient.game.GameSprite;
    import ToolForge.ForgeInventorySlot;
    import kabam.rotmg.core.model.PlayerModel;

    public class PresentControl extends ForgeControl
    {

        public static const CHRISTMAS_LETTER:int = 9018;
        public static const PRESENT:int = 6112;

        public function PresentControl(gameSprite:GameSprite)
        {
            super(gameSprite);
            drawSlot(INPUT_TARGET, 0, ANY);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(INPUT_ITEM, CHRISTMAS_LETTER);
            drawSlot(OUTPUT, PRESENT);
            setPrice(10);
        }

        override public function update(object:Object, s:String):void
        {
            //var _local_8:ItemData;
            var inventorySlot:ForgeInventorySlot;
            var player:PlayerModel;
            var inventorySlot1:ForgeInventorySlot = getSlot(INPUT_TARGET);
            var forgeInventorySlot:ForgeInventorySlot = getSlot(OUTPUT);
            var forgeInventorySlot1:ForgeInventorySlot = getSlot(INPUT_ITEM);
            //var _local_7:ItemData = inventorySlot1.getItemData();
            //_local_8 = forgeInventorySlot1.getItemData();
            //if (_local_7 == null){
            //    forgeInventorySlot.reset();
            //    return;
           // };
            //if (s == ""){
            //    s = ("Present from: " + gs.map.player_.name_);
            //};
            //_local_7.PackedItemData = _local_7.toJson();
            //_local_7.Name = s;
           // _local_7.Creator = this.gs.map.player_.name_;
            //if (((!((_local_8 == null))) && (!((_local_8.Description == null))))){
            //    _local_7.Description = _local_8.Description;
            //} else {
            //    _local_7.Description = "Mhhh, what could be in there, open it to find out.";
           // };
            //if (PRICE <= object.fame_)
            //{
                //if (inventorySlot1.itemId != -1 && forgeInventorySlot1.itemId == 9018)
                //{
                    inventorySlot = getSlot(INPUT_TARGET);
                    forgeInventorySlot.updateItemDataRaw(PRESENT, inventorySlot.slotId, inventorySlot.objectId);
                    //forgeInventorySlot.setTitle("Forge Fee:", {});
                    //forgeInventorySlot.setDescription((PRICE.toString() + " Fame"), {});
                    //object.fame_ -= PRICE;
                //}
            //}
            //else
            //{
                //forgeInventorySlot.reset();
            //}
        }

    }
}//package ToolForge.forgeControls

