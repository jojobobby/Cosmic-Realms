package com.company.assembleegameclient.ui
{
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.util.FilterUtil;
import com.company.ui.BaseSimpleText;
import com.company.util.GraphicsUtil;
import flash.display.CapsStyle;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.GraphicsStroke;
import flash.display.IGraphicsData;
import flash.display.JointStyle;
import flash.display.LineScaleMode;
import flash.display.Sprite;

public class DamageCounterFrame extends Sprite
{

    private static const CUTS:Array = [1,1,1,1];

    private static const MIN_WIDTH:int = 100;


    private var fill_:GraphicsSolidFill;

    private var path_:GraphicsPath;

    private var lineStyle_:GraphicsStroke;

    private var graphicsData_:Vector.<IGraphicsData>;

    private var backgrounds:Vector.<Sprite>;

    private var outline:Sprite;

    public var enemy:GameObject;

    private var bossName:BaseSimpleText;

    private var dmgDealt:BaseSimpleText;

    private var width_:int = 100;

    private var height_:int = 0;

    public function DamageCounterFrame(param1:GameObject)
    {
        this.backgrounds = new Vector.<Sprite>(2);
        super();
        this.enemy = param1;
        this.drawBackground();
        this.drawOverlay();
        this.drawText(false);
        this.drawDmgDealt(false);
        this.drawBackground(false);
        this.drawOverlay(false);
        this.backgrounds[0].alpha = 0.6;
        this.backgrounds[1].alpha = 0.6;
        this.outline.alpha = 0.6;
        addChild(this.backgrounds[0]);
        addChild(this.outline);
        addChild(this.backgrounds[1]);
        addChild(this.bossName);
        addChild(this.dmgDealt);
    }

    private function drawBackground(param1:Boolean = true) : void
    {
        if(this.backgrounds[0] && this.backgrounds[0].parent)
        {
            this.backgrounds[0].parent.removeChild(this.backgrounds[0]);
        }
        this.fill_ = new GraphicsSolidFill(3289650,1);
        this.path_ = new GraphicsPath(new Vector.<int>(),new Vector.<Number>());
        this.graphicsData_ = new <IGraphicsData>[this.fill_,this.path_,GraphicsUtil.END_FILL];
        this.backgrounds[0] = new Sprite();
        GraphicsUtil.clearPath(this.path_);
        GraphicsUtil.drawCutEdgeRect(0,0,this.width_,this.height_,4,CUTS,this.path_);
        this.backgrounds[0].graphics.clear();
        this.backgrounds[0].graphics.drawGraphicsData(this.graphicsData_);
        param1 && addChild(this.backgrounds[0]);
    }

    private function drawOverlay(param1:Boolean = true) : void
    {
        if(this.outline && this.outline.parent)
        {
            this.outline.parent.removeChild(this.outline);
        }
        if(this.backgrounds[1] && this.backgrounds[1].parent)
        {
            this.backgrounds[1].parent.removeChild(this.backgrounds[1]);
        }
        this.fill_ = new GraphicsSolidFill(5526612,1);
        this.lineStyle_ = new GraphicsStroke(2,false,LineScaleMode.NORMAL,CapsStyle.NONE,JointStyle.ROUND,3,this.fill_);
        this.path_ = new GraphicsPath(new Vector.<int>(),new Vector.<Number>());
        this.graphicsData_ = new <IGraphicsData>[this.lineStyle_,this.path_,GraphicsUtil.END_STROKE];
        this.outline = new Sprite();
        GraphicsUtil.clearPath(this.path_);
        GraphicsUtil.drawCutEdgeRect(0,0,this.width_,this.height_,4,CUTS,this.path_);
        this.outline.graphics.drawGraphicsData(this.graphicsData_);
        param1 && addChild(this.outline);
        this.fill_ = new GraphicsSolidFill(5526612,1);
        this.path_ = new GraphicsPath(new Vector.<int>(),new Vector.<Number>());
        this.graphicsData_ = new <IGraphicsData>[this.fill_,this.path_,GraphicsUtil.END_FILL];
        this.backgrounds[1] = new Sprite();
        GraphicsUtil.clearPath(this.path_);
        GraphicsUtil.drawCutEdgeRect(0,0,this.width_,this.height_ * 0.25,4,[1,1,0,0],this.path_);
        this.backgrounds[1].graphics.clear();
        this.backgrounds[1].graphics.drawGraphicsData(this.graphicsData_);
        param1 && addChild(this.backgrounds[1]);
    }

    private function drawText(param1:Boolean = true) : void
    {
        if(this.bossName && this.bossName.parent)
        {
            this.bossName.parent.removeChild(this.bossName);
        }
        this.bossName = new BaseSimpleText(19,11776947);
        this.bossName.htmlText = "<p align=\"center\">" + this.enemy.getName() + "</p>";
        this.bossName.updateMetrics();
        this.bossName.x = width / 2 - this.bossName.width / 2;
        this.bossName.y = 10;
        this.bossName.filters = FilterUtil.getUILabelComboFilter();
        param1 && addChild(this.bossName);
        this.width_ = Math.max(this.bossName.width + 8,MIN_WIDTH);
        this.height_ = this.height_ + this.bossName.height;
    }

    private function drawDmgDealt(param1:Boolean = true) : void
    {
        if(this.dmgDealt && this.dmgDealt.parent)
        {
            this.dmgDealt.parent.removeChild(this.dmgDealt);
        }
        this.dmgDealt = new BaseSimpleText(17,11776947);
        this.dmgDealt.htmlText = "<p align=\"center\">" + String(int(this.enemy.dmgDone / this.enemy.maxHP_ * 100)) + "%" + "</p>";
        this.dmgDealt.updateMetrics();
        this.dmgDealt.x = width / 2 - this.dmgDealt.width / 2;
        this.dmgDealt.y = this.bossName.y + this.bossName.height - this.dmgDealt.height / 2 + 8;
        this.dmgDealt.filters = FilterUtil.getUILabelComboFilter();
        param1 && addChild(this.dmgDealt);
        this.height_ = this.height_ + (this.dmgDealt.height + 9);
    }

    public function update(param1:GameObject) : void
    {
        this.enemy = param1;
        this.redraw();
    }

    private function redraw() : void
    {
        this.height_ = 0;
        this.drawText();
        this.drawDmgDealt();
        (parent as GameSprite).fixDmgCounterPos();
    }
}
}
