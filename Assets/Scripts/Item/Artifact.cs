namespace TSoft.Item
{
    public class Artifact : ItemBase<int>
    {
        public Artifact(ItemInfo info) : base(info)
        {
        }

        public Artifact(ItemInfo info, int itemValue) : base(info, itemValue)
        {
        }

        public override void SetValue(int value)
        {
        }
    }
}
