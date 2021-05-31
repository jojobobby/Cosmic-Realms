package kabam.lib.math.easing
{
    public class Expo
    {
        public function Expo()
        {
            super();
        }

        public static function easeIn(number:Number) : Number
        {
            return number == 0 ? 0 : Number(Math.pow(2,10 * (number - 1)) - 0.001);
        }

        public static function easeOut(number:Number) : Number
        {
            return number == 1 ? 1 : Number(-Math.pow(2,-10 * number) + 1);
        }

        public static function easeInOut(number:Number) : Number
        {
            if (number == 0 || number == 1)
            {
                return number;
            }
            number = number * 2;
            if (number * 2 < 1)
            {
                return 0.5 * Math.pow(2,10 * (number - 1));
            }
            return 0.5 * (-Math.pow(2,-10 * (number - 1)) + 2);
        }
    }
}