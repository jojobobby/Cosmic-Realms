package com.company.assembleegameclient.screens
{
    import com.company.assembleegameclient.objects.ObjectLibrary;
    import com.company.assembleegameclient.parameters.Parameters;
    import com.company.assembleegameclient.util.FilterUtil;
    import com.company.assembleegameclient.util.TierUtil;
    import com.company.util.PointUtil;
    import flash.display.Bitmap;
    import flash.display.BitmapData;
    import flash.display.Sprite;
    import flash.geom.Matrix;
    import io.decagames.rotmg.ui.texture.TextureParser;
    import io.decagames.rotmg.ui.labels.UILabel;
    import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
    import kabam.rotmg.core.StaticInjectorContext;
    import kabam.rotmg.text.view.BitmapTextFactory;
    import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
    import kabam.rotmg.ui.view.components.PotionSlotView;

    public class VaultSlot extends Sprite
    {
        private static const IDENTITY_MATRIX:Matrix = new Matrix();
        private static const DOSE_MATRIX:Matrix = (function ():Matrix
        {
            var _local_1:Matrix = new Matrix();
            _local_1.translate(8, 7);
            return (_local_1);
        })();
        public static const WIDTH:int = 45;
        public static const HEIGHT:int = 45;

        private var background:SliceScalingBitmap;
        private var itemBitmap:Bitmap;
        private var bitmapFactory:BitmapTextFactory;
        private var tierText:UILabel;
        private var tagContainer:Sprite;
        public var itemType:int;

        public function VaultSlot(itemType:int)
        {
            this.create();
            this.itemType = itemType;
            this.itemBitmap = new Bitmap();
            this.bitmapFactory = StaticInjectorContext.getInjector().getInstance(BitmapTextFactory);
            this.drawItem();
            this.setTierTag();
        }

        private function create():void
        {
            this.background = TextureParser.instance.getSliceScalingBitmap("UI", "popup_content_decoration", 45);
            addChild(this.background);
            this.background.height = 45;
        }

        public function drawItem():void
        {
            var bitmapData:BitmapData;
            var xml:XML;
            var bitmapData2:BitmapData;
            var bitmapData3:BitmapData;
            var type:int = this.itemType;
            if (type != -1)
            {
                bitmapData = ObjectLibrary.getRedrawnTextureFromType(type, 80, true);
                xml = ObjectLibrary.xmlLibrary_[type];
                if (xml && xml.hasOwnProperty("Doses") && this.bitmapFactory)
                {
                    bitmapData = bitmapData.clone();
                    bitmapData2 = this.bitmapFactory.make(new StaticStringBuilder(xml.Doses), 12, 0xFFFFFF, false, IDENTITY_MATRIX, false);
                    bitmapData2.applyFilter(bitmapData2, bitmapData2.rect, PointUtil.ORIGIN, PotionSlotView.READABILITY_SHADOW_2);
                    bitmapData.draw(bitmapData2, DOSE_MATRIX);
                }
                if (xml && xml.hasOwnProperty("Quantity") && this.bitmapFactory)
                {
                    bitmapData = bitmapData.clone();
                    bitmapData3 = this.bitmapFactory.make(new StaticStringBuilder(xml.Quantity), 12, 0xFFFFFF, false, IDENTITY_MATRIX, false);
                    bitmapData3.applyFilter(bitmapData3, bitmapData3.rect, PointUtil.ORIGIN, PotionSlotView.READABILITY_SHADOW_2);
                    bitmapData.draw(bitmapData3, DOSE_MATRIX);
                }
                this.itemBitmap.bitmapData = bitmapData;
                this.itemBitmap.x = ((45 / 2) - (bitmapData.width / 2));
                this.itemBitmap.y = ((45 / 2) - (bitmapData.height / 2));
                addChild(this.itemBitmap);
            }
        }

        public function setTierTag():void
        {
            var xml:XML = ObjectLibrary.xmlLibrary_[this.itemType];
            if (xml)
            {
                this.tierText = TierUtil.getTierTag(xml);
                if (this.tierText)
                {
                    if (!this.tagContainer)
                    {
                        this.tagContainer = new Sprite();
                        addChild(this.tagContainer);
                    }
                    this.tierText.filters = FilterUtil.getTextOutlineFilter();
                    this.tierText.x = (45 - this.tierText.width);
                    this.tierText.y = 27.5;
                    this.toggleTierTag(Parameters.data_.showTierTag);
                    this.tagContainer.addChild(this.tierText);
                }
            }
        }

        public function toggleTierTag(boolean:Boolean):void
        {
            if (this.tierText)
            {
                this.tierText.visible = boolean;
            }
        }
    }
}