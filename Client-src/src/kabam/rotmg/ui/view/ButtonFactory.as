package kabam.rotmg.ui.view
{
import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.texture.TextureParser;
import io.decagames.rotmg.utils.colors.GreyScale;

public class ButtonFactory
{


    public function ButtonFactory()
    {
        super();
    }

    public static function getPlayButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Play",100,false);
        return _loc1_;
    }

    public static function getTextureEditorButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Texture Editor",150);
        return _loc1_;
    }

    public static function getClassesButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Classes");
        return _loc1_;
    }

    public static function getMainButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Main");
        return _loc1_;
    }

    public static function getBattlePassButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Battle Pass");
        return _loc1_;
    }

    public static function getDoneButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"done");
        return _loc1_;
    }

    public static function getAccountButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Account");
        return _loc1_;
    }

    public static function getLegendsButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Legends");
        return _loc1_;
    }

    public static function getServersButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Servers");
        return _loc1_;
    }

    public static function getSupportButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Support");
        return _loc1_;
    }

    public static function getEditorButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Editor");
        return _loc1_;
    }

    public static function getQuitButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Quit");
        return _loc1_;
    }

    public static function getContinueButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Continue");
        return _loc1_;
    }

    public static function getResetButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Reset");
        return _loc1_;
    }

    public static function getHomeButton() : SliceScalingButton
    {
        var _loc1_:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        setDefault(_loc1_,"Home");
        return _loc1_;
    }

    private static function setDefault(param1:SliceScalingButton, param2:String, param3:int = 100, param4:Boolean = true) : void
    {
        param1.setLabel(param2,DefaultLabelFormat.questButtonCompleteLabel);
        param1.x = 0;
        param1.y = 0;
        param1.width = param3;
        if(param4)
        {
            GreyScale.greyScaleToDisplayObject(param1,true);
        }
    }
}
}
