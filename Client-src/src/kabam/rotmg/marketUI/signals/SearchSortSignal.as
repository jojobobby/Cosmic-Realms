package kabam.rotmg.marketUI.signals {
import org.osflash.signals.Signal;

public class SearchSortSignal extends Signal {
    public static var _staticInstance:SearchSortSignal;

    public function SearchSortSignal() {
        super(String);
        _staticInstance = this;
    }
}
}
