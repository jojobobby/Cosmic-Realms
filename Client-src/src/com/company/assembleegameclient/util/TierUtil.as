package com.company.assembleegameclient.util
{
import flash.filters.GlowFilter;

import io.decagames.rotmg.ui.labels.UILabel;
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

public class TierUtil
{
    private static const STANDARD_OUTLINE_FILTER:Array = [new GlowFilter(0, 1, 2, 2, 10, 1)];

    public static function getTierTag(_arg_1:XML, _arg_2:int=12):UILabel
    {
        var _local_9:UILabel;
        var _local_10:Number;
        var _local_11:String;
        var _local_3:* = !isPet(_arg_1);
        var _local_4:* = !_arg_1.hasOwnProperty("Consumable");
        var _local_5:* = !_arg_1.hasOwnProperty("InvUse");
        var _local_6:* = !_arg_1.hasOwnProperty("Treasure");
        var _local_7:* = !_arg_1.hasOwnProperty("PetFood");
        var x2 = (_arg_1.BagType);
        var x = (_arg_1.SlotType);
        var _local_8:Boolean = _arg_1.hasOwnProperty("Tier");
        if ((((((_local_3) && (_local_4)) && (_local_5)) && (_local_6)) && (_local_7)))
        {
            _local_9 = new UILabel();
            if (_local_8)
            {
                _local_10 = 0xFFFFFF;
                _local_11 = ("T" + _arg_1.Tier);
            }
            else
            {

                if (x2 == 11) {

                    _local_10 = 0x00ff00;
                    _local_11 = "F ";
                }
                else if (_arg_1.hasOwnProperty("Radiant"))
                {
                    _local_10 = 0x7540AA;
                    _local_11 = "RT";
                }
                else if (_arg_1.hasOwnProperty("@setType"))
                {
                    _local_10 = 0xFF9900;
                    _local_11 = "ST";
                }
                else if (_arg_1.hasOwnProperty("LG"))
                {
                    _local_10 = 0xfce303;
                    _local_11 = "LG";
                }
                else if (_arg_1.hasOwnProperty("ST"))
                {
                    _local_10 = 0xE90000;
                    _local_11 = "MY";
                }
                else if (_arg_1.hasOwnProperty("MLG"))
                {
                    _local_10 = 0xD5FBFC;
                    _local_11 = "LG";
                }
                else if (_arg_1.hasOwnProperty("Lunar"))
                {
                    _local_10 = 0xCACACA;
                    _local_11 = "LT";
                }
                else if (_arg_1.hasOwnProperty("MY"))
                {
                    _local_10 = 0xfce303;//0xaf0847
                    _local_11 = "LG";
                }
                else if (_arg_1.hasOwnProperty("BG"))
                {
                    _local_10 = 0xDC143C;
                    _local_11 = "BG";
                }
                else if (_arg_1.hasOwnProperty("Lunar"))
                {
                    _local_10 = 0xCACACA;
                    _local_11 = "UT";
                }
                else if (_arg_1.hasOwnProperty("Epic"))
                {
                    _local_10 = 0x8A2BE2;
                    _local_11 = "UT";
                }
                else if (x != 10)
                {
                    _local_10 = 0xB200FF;
                    _local_11 = "UT";
                }
            }
            _local_9.text = _local_11;
            DefaultLabelFormat.tierLevelLabel(_local_9, _arg_2, _local_10);
            return _local_9;
        }
        return null;
    }

    public static function isPet(itemDataXML:XML):Boolean
    {
        var activateTags:XMLList;
        activateTags = itemDataXML.Activate.(text() == "PermaPet");
        return activateTags.length() >= 1;
    }

    public static function getTextOutlineFilter():Array{
        return (STANDARD_OUTLINE_FILTER);
    }
}
}