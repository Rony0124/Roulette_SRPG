using System.Collections.Generic;

namespace HF.AI
{
    public class NodeState
    {
        public int tdepth;      //Depth in number of turns
        public int taction;     //How many orders in current turn
        public int sort_min;    //Sorting minimum value, orders below this value will be ignored to avoid calculate both path A -> B and path B -> A
        public int hvalue;      //Heuristic value, this AI tries to maximize it, opponent tries to minimize it
        public int alpha;       //Highest heuristic reached by the AI player, used for optimization and ignore some tree branch
        public int beta;        //Lowest heuristic reached by the opponent player, used for optimization and ignore some tree branch

        public AIAction last_action = null;
        public int current_player;

        public NodeState parent;
        public NodeState best_child = null;
        public List<NodeState> childs = new List<NodeState>();

        public NodeState() { }

        public NodeState(NodeState parent, int player_id, int turn_depth, int turn_action, int turn_sort)
        {
            this.parent = parent;
            this.current_player = player_id;
            this.tdepth = turn_depth;
            this.taction = turn_action;
            this.sort_min = turn_sort;
        }

        public void Clear()
        {
            last_action = null;
            best_child = null;
            parent = null;
            childs.Clear();
        }
    }
}
