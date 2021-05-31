package kabam.lib.math.easing
{
    public class Back
    {
        public function Back()
        {
            super();
        }

        public static function easeIn(number:Number) : Number
        {
            return number * number * (2.70158 * number - 1.70158);
        }

        public static function easeOut(number:Number) : Number
        {
            number--;
            return number * number * (2.70158 * number + 1.70158) + 1;
        }

        public static function easeInOut(number:Number) : Number
        {
            number = number * 2;
            if (number * 2 < 1)
            {
                return 0.5 * number * number * (3.5949095 * number - 2.5949095);
            }
            number = number - 2;
            return 0.5 * ((number - 2) * number * (3.5949095 * number + 2.5949095) + 2);
        }
    }
}