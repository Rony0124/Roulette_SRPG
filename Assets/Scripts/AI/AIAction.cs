using HF.GamePlay;

namespace HF.AI
{
    public class AIAction
    {
        public ushort type;
        
        public int score;     //Score to determine which orders get cut and ignored
        public int sort;     //Orders must be executed in sort order
        public bool valid;

        public AIAction() { }
        public AIAction(ushort t) { type = t; }
        
        public string GetText(Game data)
        {
            //string txt = GameAction.GetString(type);
            string txt = type.ToString();
            return txt;
        }
        
        public void Clear()
        {
            type = 0;
            valid = false;
        }

        public static AIAction None {
            get { 
                var a = new AIAction {
                        type = 0 
                };
                return a;
            }
        }
    }
}
