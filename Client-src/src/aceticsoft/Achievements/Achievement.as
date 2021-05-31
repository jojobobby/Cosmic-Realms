/**
 * Created by Fabian on 24.01.2015.
 */
package aceticsoft.Achievements
{
    import com.company.assembleegameclient.game.AGameSprite;
    import com.company.ui.BaseSimpleText;
    import com.gskinner.motion.GTween;
    import flash.display.Bitmap;
    import flash.display.DisplayObject;
    import flash.display.GradientType;
    import flash.display.Sprite;
    import flash.events.Event;
    import flash.events.TimerEvent;
    import flash.geom.Matrix;
    import flash.utils.Timer;
    import flash.utils.getTimer;

    public class Achievement extends Sprite
    {
        [Embed(source="LegendaryLootTrue.png")]
        protected static var LegendaryLoot:Class;
        [Embed(source="1422120678_Medal.png")]
        protected static var acIcon:Class;
        [Embed(source="Mastery1_9.png")]
        protected static var Mastery1_9Icon:Class;//Mastery1_9
        [Embed(source="devwarlt.png")]
        protected static var devwarlIcon:Class;
        [Embed(source="Mastery10_19.png")]
        protected static var Mastery10_19Icon:Class;
        protected static const WIDTH:int = 350;
        protected static const HEIGHT:int = 100;
        public static var CurrentAchievements:Vector.<Achievement> = new Vector.<Achievement>();
        public static var currentAchievement:Achievement;
        protected var gs_:AGameSprite;
        protected var oldTime:int = -1;
        private var title:String;
        private var desc:String;
        private var iconId:int;

        function Achievement(gameSprite:AGameSprite, title:String, desc:String, iconId:int)
        {
            this.gs_ = gameSprite;
            this.title = title;
            this.desc = desc;
            this.iconId = iconId;
            this.addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            this.y = -(HEIGHT + 1);
            this.x = 1; //x: 1 because the border is 2 so 2 / 2 is 1;
            if (this.gs_.map.stage != null)
            {
                if (currentAchievement == null)
                {
                    currentAchievement = this;
                    this.gs_.map.stage.addChild(this);
                    init();
                }
                else
                {
                    CurrentAchievements.push(this);
                }
            }
            else
            {
                CurrentAchievements = new Vector.<Achievement>();
                currentAchievement = null;
            }
        }

        protected function init():void
        {
            var ic:Bitmap;



            if (iconId == 0)
            {
                ic = new acIcon();
            }
            if (iconId == 1)
            {
                ic = new devwarlIcon();
            }
            if (iconId == 2)
            {
                ic = new Mastery1_9Icon();
            }
            if (iconId == 3)
            {
                ic = new Mastery10_19Icon();
            }
            if (iconId == 55)
            {
                ic = new LegendaryLoot();
            }
            ic.x = 2;
            ic.y = (HEIGHT / 2) - (ic.height / 2);
            addChild(ic);
            var text:BaseSimpleText = new BaseSimpleText(20, 0xffffff);
            text.setBold(true);
            text.text = title;
            text.x = 100;
            text.y = 2;
            text.updateMetrics();
            addChild(text);
            var descText:BaseSimpleText = new BaseSimpleText(16, 0xffffff, false, 250);
            descText.multiline = true;
            descText.wordWrap = true;
            descText.text = desc;
            descText.x = 100;
            descText.y = 2 + text.height;
            descText.updateMetrics();
            addChild(descText);
            fadeIn();
        }

        protected function fadeIn():void
        {
            new GTween(this, 0.5, {"y": 1}); //Y: 1 because the border is 2 so 2 / 2 is 1;
            var timer:Timer = new Timer(5 * 1000, 1);
            timer.addEventListener(TimerEvent.TIMER_COMPLETE, this.fadeOut);
            timer.start();
        }

        private function fadeOut(event:TimerEvent):void
        {
            var gTween:GTween = new GTween(this, 0.5, {"y": -(HEIGHT - 1)});
            gTween.onComplete = function (tween:GTween):void
            {
                parent.removeChild(tween.target as DisplayObject);
            }
        }

        protected function onAddedToStage(event:Event):void
        {
            this.addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
            this.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        }

        protected function onEnterFrame(event:Event):void {
            var time:int;
            if ((time = ((getTimer() / 10) % 360)) != oldTime)
            {
                graphics.clear();
                graphics.beginFill(0x2B2B2B, 0.8);
                graphics.drawRect(0, 0, 350, 100);
                graphics.endFill();
                var gradientMatrix:Matrix = new Matrix();
                gradientMatrix.createGradientBox(350, 100, (Math.PI / 180) * time, 0, 0);
                graphics.lineStyle(2);
                graphics.lineGradientStyle(GradientType.LINEAR, [0xFFFFFF, 0xFFFFFF, 0xFFFFFF], [1.0, 1.0, 1.0], [0, 127, 255], gradientMatrix);
                graphics.drawRect(0, 0, 350, 100);
                graphics.endFill();
            }
        }

        protected function onRemovedFromStage(event:Event):void
        {
            if (this.gs_.map.stage != null)
            {
                var nextAchievement:Achievement;
                if ((nextAchievement = CurrentAchievements.shift()) != null)
                {
                    currentAchievement = nextAchievement;
                    this.gs_.map.stage.addChild(nextAchievement);
                    nextAchievement.init();
                }
                else
                {
                    currentAchievement = null;
                }
            }
            else
            {
                CurrentAchievements = new Vector.<Achievement>();
                currentAchievement = null;
            }
        }
    }
}