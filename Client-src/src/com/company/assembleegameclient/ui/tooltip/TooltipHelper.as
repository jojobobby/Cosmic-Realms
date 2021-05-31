package com.company.assembleegameclient.ui.tooltip
{
    public class TooltipHelper
    {

        public static const BETTER_COLOR:uint = 0x00FF00;//3a9fbf
        public static const WORSE_COLOR:uint = 0xFF0000;
        public static const NO_DIFF_COLOR:uint = 16777103;
        public static const WisMod:uint = 0x3a9fbf;
        public static const NO_DIFF_COLOR_INACTIVE:uint = 6842444;
        public static const WIS_BONUS_COLOR:uint = 4219875;
        public static const UNTIERED_COLOR:uint = 0x0070dd;
        public static const SET_COLOR:uint = 0xff8000;
        public static const LG:String = "#bd4e5e";
        public static const BG:String = "#DC143C";
        public static const SET_COLOR_INACTIVE:uint = 6835752;

        public static function wrapInFontTag(text:String, color:String) : String
        {
            var tagStr:String = "<font color=\"" + color + "\">" + text + "</font>";
            return tagStr;
        }

        public static function getOpenTag(_arg1:uint):String
        {
            return ((('<font color="#' + _arg1.toString(16)) + '">'));
        }

        public static function getCloseTag():String
        {
            return "</font>";
        }

        public static function getFormattedRangeString(_arg1:Number):String
        {
            var _local2:Number = _arg1 - int(_arg1);
            return ((((int((_local2 * 10))) == 0) ? int(_arg1).toString() : _arg1.toFixed(1)));
        }

        public static function getTextColor(_arg1:Number):uint
        {
            if (_arg1 < 0)
            {
                return WORSE_COLOR;
            }
            if (_arg1 > 0)
            {
                return BETTER_COLOR;
            }
            return NO_DIFF_COLOR;
        }
    }
}