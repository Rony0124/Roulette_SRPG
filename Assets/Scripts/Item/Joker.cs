namespace TSoft.Item
{
    public class Joker : ItemBase<int>
    {
        public Joker(ItemInfo info) : base(info)
        {
        }

        public Joker(ItemInfo info, int itemValue) : base(info, itemValue)
        {
        }

        public override void SetValue(int value)
        {
        }
    }
}
