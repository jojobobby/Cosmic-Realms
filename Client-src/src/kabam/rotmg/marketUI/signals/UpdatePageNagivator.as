package kabam.rotmg.marketUI.signals {
import org.osflash.signals.Signal;

public class UpdatePageNagivator extends Signal {
    public static var _staticInstance:UpdatePageNagivator;

    public function UpdatePageNagivator() {
        super(int, int);
        _staticInstance = this;
    }

}
}
