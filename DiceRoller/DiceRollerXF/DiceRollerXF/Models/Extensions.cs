using Xamarin.Forms;

namespace DiceRollerXF
{
    public static class Extensions
    {
        static bool _isRuning;
        public async static void AnimateClick(this VisualElement element)
        {
            if (_isRuning)
                return;
            _isRuning = true;
            await element.ScaleTo(0.9);
            await element.ScaleTo(1);
            _isRuning = false;
        }
    }
}
