package com.company.util
{
    import flash.display.BitmapData;
    import flash.geom.Matrix;
    import flash.geom.Point;
    import flash.geom.Rectangle;
    import flash.utils.Dictionary;

    public class BitmapUtil
    {
        public function BitmapUtil()
        {
            super();
        }

        public static function resize(data:BitmapData, width:int, height:int):BitmapData {
            var scaleX:Number = width / data.width;
            var scaleY:Number = height / data.height;
            var scaleMatrix:Matrix = new Matrix();
            scaleMatrix.scale(scaleX, scaleY);

            var toBitmap:BitmapData = new BitmapData(width, height, true, 0x000000);
            toBitmap.draw(data, scaleMatrix);

            return toBitmap;
        }



        public static function mirror(bitmapData:BitmapData, width:int = 0):BitmapData
        {
            var y:int;
            if (width == 0)
            {
                width = bitmapData.width;
            }
            var mirrored:BitmapData = new BitmapData(bitmapData.width, bitmapData.height, true, 0);
            for (var x:int = 0; x < width; x++)
            {
                for (y = 0; y < bitmapData.height; y++)
                {
                    mirrored.setPixel32(width - x - 1, y, bitmapData.getPixel32(x, y));
                }
            }
            return mirrored;
        }

        public static function rotateBitmapData(bitmapData:BitmapData, clockwiseTurns:int):BitmapData
        {
            var matrix:Matrix = new Matrix();
            matrix.translate(-bitmapData.width / 2, -bitmapData.height / 2);
            matrix.rotate(clockwiseTurns * Math.PI / 2);
            matrix.translate(bitmapData.height / 2, bitmapData.width / 2);
            var rotated:BitmapData = new BitmapData(bitmapData.height, bitmapData.width, true, 0);
            rotated.draw(bitmapData, matrix);
            return rotated;
        }

        public static function cropToBitmapData(bitmapData:BitmapData, x:int, y:int, width:int, height:int, fillColor:uint):BitmapData
        {
            var cropped:BitmapData = new BitmapData(width, height, fillColor);
            cropped.copyPixels(bitmapData, new Rectangle(x, y, width, height), new Point(0, 0));
            return cropped;
        }

        public static function amountTransparent(bitmapData:BitmapData):Number
        {
            var y:int = 0;
            var alpha:int = 0;
            var trans:int = 0;
            for (var x:int = 0; x < bitmapData.width; x++)
            {
                for (y = 0; y < bitmapData.height; y++)
                {
                    alpha = bitmapData.getPixel32(x, y) & 0xFF000000;
                    if (alpha == 0)
                    {
                        trans++;
                    }
                }
            }
            return trans / (bitmapData.width * bitmapData.height);
        }

        public static function mostCommonColor(bitmapData:BitmapData):uint
        {
            var color:*;
            var colorStr:String;
            var y:int;
            var count:int;
            var colors_:Dictionary = new Dictionary();
            var x:int = 0;
            while (x < bitmapData.width)
            {
                y = 0;
                while (y < bitmapData.width)
                {
                    color = bitmapData.getPixel32(x, y);
                    if ((color & 0xFF000000) != 0)
                    {
                        if (!colors_.hasOwnProperty(color))
                        {
                            colors_[color] = 1;
                        }
                        else
                        {
                            var _local10:* = colors_;
                            var _local11:* = color;
                            var _local12:* = _local10[_local11] + 1;
                            _local10[_local11] = _local12;
                        }
                    }
                    y++;
                }
                x++;
            }
            var bestColor:uint = 0;
            var bestCount:uint = 0;
            for (colorStr in colors_)
            {
                color = uint(colorStr);
                count = colors_[colorStr];
                if (count > bestCount || count == bestCount && color > bestColor)
                {
                    bestColor = color;
                    bestCount = count;
                }
            }
            return (bestColor);
        }
/**
        public static function lineOfSight(_arg1:BitmapData, _arg2:IntPoint, _arg3:IntPoint):Boolean {
            var _local11:int;
            var _local19:int;
            var _local20:int;
            var _local21:int;
            var _local4:int = _arg1.width;
            var _local5:int = _arg1.height;
            var _local6:int = _arg2.x();
            var _local7:int = _arg2.y();
            var _local8:int = _arg3.x();
            var _local9:int = _arg3.y();
            var _local10 = ((((_local7 > _local9)) ? (_local7 - _local9) : (_local9 - _local7)) > (((_local6 > _local8)) ? (_local6 - _local8) : (_local8 - _local6)));
            if (_local10) {
                _local11 = _local6;
                _local6 = _local7;
                _local7 = _local11;
                _local11 = _local8;
                _local8 = _local9;
                _local9 = _local11;
                _local11 = _local4;
                _local4 = _local5;
                _local5 = _local11;
            }
            if (_local6 > _local8) {
                _local11 = _local6;
                _local6 = _local8;
                _local8 = _local11;
                _local11 = _local7;
                _local7 = _local9;
                _local9 = _local11;
            }
            var _local12:int = (_local8 - _local6);
            var _local13:int = (((_local7 > _local9)) ? (_local7 - _local9) : (_local9 - _local7));
            var _local14:int = (-((_local12 + 1)) / 2);
            var _local15:int = (((_local7) > _local9) ? -1 : 1);
            var _local16:int = (((_local8 > (_local4 - 1))) ? (_local4 - 1) : _local8);
            var _local17:int = _local7;
            var _local18:int = _local6;
            if (_local18 < 0) {
                _local14 = (_local14 + (_local13 * -(_local18)));
                if (_local14 >= 0) {
                    _local19 = ((_local14 / _local12) + 1);
                    _local17 = (_local17 + (_local15 * _local19));
                    _local14 = (_local14 - (_local19 * _local12));
                }
                _local18 = 0;
            }
            if ((((((_local15 > 0)) && ((_local17 < 0)))) || ((((_local15 < 0)) && ((_local17 >= _local5)))))) {
                _local20 = (((_local15 > 0)) ? (-(_local17) - 1) : (_local17 - _local5));
                _local14 = (_local14 - (_local12 * _local20));
                _local21 = (-(_local14) / _local13);
                _local18 = (_local18 + _local21);
                _local14 = (_local14 + (_local21 * _local13));
                _local17 = (_local17 + (_local20 * _local15));
            }
            while (_local18 <= _local16) {
                if ((((((_local15 > 0)) && ((_local17 >= _local5)))) || ((((_local15 < 0)) && ((_local17 < 0)))))) break;
                if (_local10) {
                    if ((((((_local17 >= 0)) && ((_local17 < _local5)))) && ((_arg1.getPixel(_local17, _local18) == 0)))) {
                        return (false);
                    }
                }
                else {
                    if ((((((_local17 >= 0)) && ((_local17 < _local5)))) && ((_arg1.getPixel(_local18, _local17) == 0)))) {
                        return (false);
                    }
                }
                _local14 = (_local14 + _local13);
                if (_local14 >= 0) {
                    _local17 = (_local17 + _local15);
                    _local14 = (_local14 - _local12);
                }
                _local18++;
            }
            return (true);
        }
        **/
    }
}
class StaticEnforcer {


}