package com.company.assembleegameclient.ui.panels.itemgrids.itemtiles
{
    import com.company.assembleegameclient.objects.ObjectLibrary;
    import com.company.assembleegameclient.objects.Player;
    import com.company.assembleegameclient.ui.panels.itemgrids.ItemGrid;
    import com.company.assembleegameclient.util.FilterUtil;
    import com.company.assembleegameclient.util.TierUtil;
    import com.company.util.GraphicsUtil;
    import flash.display.GraphicsPath;
    import flash.display.GraphicsSolidFill;
    import flash.display.IGraphicsData;
    import flash.display.Shape;
    import flash.display.Sprite;
    import io.decagames.rotmg.ui.labels.UILabel;
    import kabam.rotmg.constants.ItemConstants;

    public class ItemTile extends Sprite
    {
        public static const WIDTH:int = 40;
        public static const HEIGHT:int = 40;
        public static const BORDER:int = 4;

        private var fill_:GraphicsSolidFill = new GraphicsSolidFill(getBackgroundColor(),1);
        private var path_:GraphicsPath = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());
        private var graphicsData_:Vector.<IGraphicsData> = new <IGraphicsData>[fill_, path_, GraphicsUtil.END_FILL];
        private var restrictedUseIndicator:Shape;
        public var itemSprite:ItemTileSprite;
        public var tileId:int;
        public var ownerGrid:ItemGrid;
        public var blockingItemUpdates:Boolean;
        private var tierText:UILabel;
        private var itemContainer:Sprite;
        private var tagContainer:Sprite;
        private var isItemUsable:Boolean;

        public function ItemTile(_arg1:int, _arg2:ItemGrid)
        {
            super();
            this.tileId = _arg1;
            this.ownerGrid = _arg2;
            this.restrictedUseIndicator = new Shape();
            addChild(this.restrictedUseIndicator);
            this.setItemSprite(new ItemTileSprite());
        }

        public function drawBackground(_arg1:Array):void
        {
            GraphicsUtil.clearPath(this.path_);
            GraphicsUtil.drawCutEdgeRect(0, 0, WIDTH, HEIGHT, 4, _arg1, this.path_);
            graphics.clear();
            graphics.drawGraphicsData(this.graphicsData_);
            var _local2:GraphicsSolidFill = new GraphicsSolidFill(6036765, 1);
            GraphicsUtil.clearPath(this.path_);
            var _local3:Vector.<IGraphicsData> = new <IGraphicsData>[_local2, this.path_, GraphicsUtil.END_FILL];
            GraphicsUtil.drawCutEdgeRect(0, 0, WIDTH, HEIGHT, 4, _arg1, this.path_);
            this.restrictedUseIndicator.graphics.drawGraphicsData(_local3);
            this.restrictedUseIndicator.cacheAsBitmap = true;
            this.restrictedUseIndicator.visible = false;
        }

        public function setItem(itemId:int):Boolean
        {
            if (itemId == this.itemSprite.itemId)
            {
                return false;
            }
            if (this.blockingItemUpdates)
            {
                return true;
            }
            this.itemSprite.setType(itemId);
            this.setTierTag();
            this.updateUseability(this.ownerGrid.curPlayer);
            return true;
        }

        public function setItemSprite(_arg1:ItemTileSprite):void
        {
            if (!this.itemContainer)
            {
                this.itemContainer = new Sprite();
                addChild(this.itemContainer);
            }
            this.itemSprite = _arg1;
            this.itemSprite.x = WIDTH / 2;
            this.itemSprite.y = HEIGHT / 2;
            this.itemContainer.addChild(this.itemSprite);
        }

        public function updateUseability(player:Player):void
        {
            var itemId:int = this.itemSprite.itemId;
            if ((((itemId >= 0x9000)) && ((itemId < 0xF000))))
            {
                itemId = 36863;
            }
            if (this.itemSprite.itemId != ItemConstants.NO_ITEM)
            {
                this.restrictedUseIndicator.visible = !(ObjectLibrary.isUsableByPlayer(itemId, player));
            }
            else
            {
                this.restrictedUseIndicator.visible = false;
            }
        }

        public function canHoldItem(_arg1:int):Boolean
        {
            return true;
        }

        public function resetItemPosition():void
        {
            this.setItemSprite(this.itemSprite);
        }

        public function getItemId():int
        {
            if ((((this.itemSprite.itemId >= 0x9000)) && ((this.itemSprite.itemId < 0xF000)))) {
                return 36863;
            }
            return this.itemSprite.itemId;
        }

        protected function getBackgroundColor():int
        {
            return 0x545454;
        }

        public function setTierTag():void
        {
            this.clearTierTag();
            var item:XML = ObjectLibrary.xmlLibrary_[this.itemSprite.itemId];
            if (item)
            {
                this.tierText = TierUtil.getTierTag(item);
                if (this.tierText != null)
                {
                    if (!this.tagContainer)
                    {
                        this.tagContainer = new Sprite();
                        addChild(this.tagContainer);
                    }
                    this.tierText.filters = FilterUtil.getTextOutlineFilter();
                    this.tierText.x = WIDTH - this.tierText.width;
                    this.tierText.y = (HEIGHT / 2) + 5;
                    this.toggleTierTag(true);
                    this.tagContainer.addChild(this.tierText);
                }
            }
        }

        private function clearTierTag():void
        {
            if ((((this.tierText) && (this.tagContainer)) && (this.tagContainer.contains(this.tierText))))
            {
                this.tagContainer.removeChild(this.tierText);
                this.tierText = null;
            }
        }

        public function toggleTierTag(_arg1:Boolean):void
        {
            if (this.tierText)
            {
                this.tierText.visible = _arg1;
            }
        }

        protected function toggleDragState(_arg1:Boolean):void
        {
            if (this.tierText)
            {
                this.tierText.visible = _arg1;
            }
            if (((!(this.isItemUsable)) && (!(_arg1))))
            {
                this.restrictedUseIndicator.visible = _arg1;
            }
        }
    }
}