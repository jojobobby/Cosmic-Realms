/**
 * Created by Fabian on 24.01.2015.
 */
package aceticsoft.Achievements
{
    import com.company.assembleegameclient.game.AGameSprite;

    public class BasicAchievement extends Achievement
    {
        public function BasicAchievement(gameSprite:AGameSprite, title:String, desc:String, iconId:int)
        {
            super(gameSprite, title, desc, iconId);
        }
    }
}