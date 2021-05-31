/**
 * Credits to Nillys Realm
 */
package kabam.rotmg.emotes
{
    import flash.display.Sprite;
    import flash.display.BitmapData;
    import flash.geom.Matrix;
    import flash.display.Shape;
    import flash.display.Bitmap;
    import flash.filters.GlowFilter;

    public class Emote extends Sprite
    {
        private var emoteName:String;
        private var bitmapData:BitmapData;
        private var scale:Number;
        private var hq:Boolean;

        public function Emote(_arg1:String, _arg2:BitmapData, _arg3:Number, _arg4:Boolean)
        {
            this.emoteName = _arg1;
            this.bitmapData = _arg2;
            this.scale = _arg3;
            this.hq = _arg4;
            var bitmapData:BitmapData = _arg2;
            var matrix:Matrix = new Matrix();
            matrix.scale(_arg3, _arg3);
            var bitmapData2:BitmapData = new BitmapData(Math.floor((bitmapData.width * _arg3)), Math.floor((bitmapData.height * _arg3)), true, 0);
            bitmapData2.draw(bitmapData, matrix, null, null, null, _arg4);
            var shape:Shape = new Shape();
            shape.graphics.beginBitmapFill(bitmapData, matrix, false, true);
            shape.graphics.lineStyle(0, 0, 0);
            shape.graphics.drawRect(0, 0, bitmapData2.width, bitmapData2.height);
            shape.graphics.endFill();
            bitmapData2.draw(shape);
            var _local9:Bitmap = new Bitmap(bitmapData2);
            _local9.filters = (_arg4) ? [] : [new GlowFilter(0, 1, 6, 6, 4)];
            _local9.y = -2;
            addChild(_local9);
        }

        public function clone():Emote
        {
            return new Emote(this.emoteName, this.bitmapData, this.scale, this.hq);
        }
    }
}