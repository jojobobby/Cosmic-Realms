package com.company.assembleegameclient.util
{
    public class ParseUtils
    {
        public static function parseIntList(string:String):Vector.<int>
        {
            var i:int;
            var split:Array = string.split(",");
            var length:int = split.length;
            if (string.length <= 0)
            {
                return null;
            }
            var vector:Vector.<int> = new Vector.<int>(length, true);
            i = 0;
            while (i < length)
            {
                vector[i] = split[i];
                i++;
            }
            return vector;
        }
    }
}