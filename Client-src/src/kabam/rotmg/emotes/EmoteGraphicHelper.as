/**
 * Credits to Nillys Realm
 */
package kabam.rotmg.emotes
{
    import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
    import kabam.rotmg.text.view.stringBuilder.StringBuilder;
    import flash.text.TextField;
    import flash.display.DisplayObject;
    import flash.text.TextFormat;
    import flash.display.Sprite;
    import flash.text.TextFieldAutoSize;

    public class EmoteGraphicHelper {

        private static const tField:TextField = makeTextField();

        private var buffer:Vector.<DisplayObject>;

        public function EmoteGraphicHelper()
        {
            this.buffer = new Vector.<DisplayObject>();
        }

        private static function makeTextField():TextField
        {
            var _local1:TextField = new TextField();
            var _local2:TextFormat = new TextFormat();
            _local2.size = 14;
            _local2.bold = false;
            _local1.defaultTextFormat = _local2;
            return _local1;
        }


        public function getSpeechBalloonText(_arg1:String, _arg2:Boolean, _arg3:uint):Sprite
        {
            this.add(_arg1, _arg2, _arg3);
            return new Drawer(this.buffer, 150, 17);
        }

        private function getAllWords(_arg1:String):Array
        {
            return _arg1.split(" ");
        }

        private function add(_arg1:String, _arg2:Boolean, _arg3:uint):void
        {
            var _local4:StringBuilder;
            var _local5:String;
            for each (_local5 in this.getAllWords(_arg1))
            {
                if (Emotes.hasEmote(_local5))
                {
                    this.buffer.push(Emotes.getEmote(_local5));
                }
                else if (_arg1.indexOf("url::") >= 0)
                {
                    return;
                }
                else
                {
                    _local4 = new StaticStringBuilder(_local5);
                    this.buffer.push(this.makeNormalText(_local4, _arg2, _arg3));
                }
            }
        }

        private function makeNormalText(_arg1:StringBuilder, _arg2:Boolean, _arg3:uint):TextField
        {
            var _local4:TextField;
            _local4 = new TextField();
            _local4.autoSize = TextFieldAutoSize.LEFT;
            _local4.embedFonts = true;
            var _local5:TextFormat = new TextFormat();
            _local5.font = "Myriad Pro";
            _local5.size = 14;
            _local5.bold = _arg2;
            _local5.color = _arg3;
            _local4.defaultTextFormat = _local5;
            _local4.selectable = false;
            _local4.mouseEnabled = false;
            _local4.text = _arg1.getString();
            if (_local4.textWidth > 150)
            {
                _local4.multiline = true;
                _local4.wordWrap = true;
                _local4.width = 150;
            }
            return _local4;
        }
    }
}

import flash.display.Sprite;
import flash.display.DisplayObject;
import flash.geom.Rectangle;

import kabam.rotmg.emotes.Emote;

class Drawer extends Sprite
{
    private var maxWidth:int;
    private var list:Vector.<DisplayObject>;
    private var count:uint;
    private var lineHeight:uint;

    public function Drawer(_arg1:Vector.<DisplayObject>, _arg2:int, _arg3:int)
    {
        this.maxWidth = _arg2;
        this.lineHeight = _arg3;
        this.list = _arg1;
        this.count = _arg1.length;
        this.layoutItems();
        this.addItems();
    }

    private function layoutItems():void
    {
        var _local1:int;
        var _local2:DisplayObject;
        var _local3:Rectangle;
        var _local4:int;
        var _local5:int;
        _local1 = 0;
        while (_local5 < this.count)
        {
            _local2 = this.list[_local5];
            _local3 = _local2.getRect(_local2);
            _local2.x = _local1;
            _local2.y = -(_local3.height);
            if ((_local1 + _local3.width) > this.maxWidth)
            {
                _local2.x = 0;
                _local1 = 0;
                _local4 = 0;
                while (_local4 < _local5)
                {
                    this.list[_local4].y = (this.list[_local4].y - this.lineHeight);
                    _local4++;
                }
            }
            _local1 = (_local1 + (((_local2 is Emote)) ? (_local3.width + 2) : _local3.width));
            _local5++;
        }
    }

    private function addItems():void
    {
        var _local1:DisplayObject;
        for each (_local1 in this.list)
        {
            addChild(_local1);
        }
    }
}