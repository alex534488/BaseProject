
namespace CCC.Manager
{
    public class SpriteBankManager : BaseManager
    {
        public SpriteBank bank;
        public override void Init()
        {
            base.Init();
            bank.SetAsInstance();
            CompleteInit();
        }
    }
}
