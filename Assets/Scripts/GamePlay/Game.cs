using System.Collections.Generic;

namespace HF.GamePlay
{
    [System.Serializable]
    public enum GameState
    {
        Connecting = 0, //Players are not connected
        Play = 20,      //Game is being played
        GameEnded = 99,
    }

    [System.Serializable]
    public enum GamePhase
    {
        None = 0,
        StartTurn = 10, //Start of turn resolution
        Main = 20,      //Main play phase
        EndTurn = 30,   //End of turn resolutions
    }
    
    [System.Serializable]
    public enum SelectorType
    {
        None = 0,
        SelectTarget = 10,
        SelectorCard = 20,
        SelectorChoice = 30,
        SelectorCost = 40,
    }
    
    [System.Serializable]
    public class Game
    {
        public string game_uid;
        public GameSettings settings;
        
        //Game state
        public int first_player = 0;
        public int current_player = 0;
        public int turn_count = 0;
        public float turn_timer = 0f;
        
        public GameState state = GameState.Connecting;
        public GamePhase phase = GamePhase.None;
        
        //Players
        public Player[] players;
        
        //Selector
        public SelectorType selector = SelectorType.None;
        public int selector_player_id = 0;
        
        public Game() { }
        
        public Game(string uid, int playerNum)
        {
            game_uid = uid;
            
            players = new Player[playerNum];
            for (int i = 0; i < playerNum; i++)
                players[i] = new Player(i);
            
            settings = GameSettings.Default;
        }
        
        public bool IsPlayerTurn(Player player)
        {
            return IsPlayerActionTurn(player) || IsPlayerSelectorTurn(player);
        }
        
        public bool IsPlayerActionTurn(Player player)
        {
            return player != null && current_player == player.player_id 
                                  && state == GameState.Play && phase == GamePhase.Main && selector == SelectorType.None;
        }
        
        public bool IsPlayerSelectorTurn(Player player)
        {
            return player != null && selector_player_id == player.player_id 
                                  && state == GameState.Play && phase == GamePhase.Main && selector != SelectorType.None;
        }
        
        public Player GetPlayer(int id)
        {
            if (id >= 0 && id < players.Length)
                return players[id];
            
            return null;
        }
        
        public Player GetActivePlayer()
        {
            return GetPlayer(current_player);
        }

        
        public Player GetOpponentPlayer(int id)
        {
            int oid = id == 0 ? 1 : 0;
            return GetPlayer(oid);
        }
        
        public bool HasEnded()
        {
            return state == GameState.GameEnded;
        }
        
        public bool CanPlayCard(Card card)
        {
            if (card == null)
                return false;
            
            return true;
        }
        
        public bool CanPlayPattern(CardPattern card)
        {
            if (card == null)
                return false;
            
            return true;
        }
        
        public virtual bool CanAttackTarget(Player attacker, Player target, bool skip_cost = false)
        {
            if (attacker == null || target == null)
                return false;

            if (attacker.player_id == target.player_id)
                return false; //Cant attack same player

            return true;
        }
        
        public static Game CloneNew(Game source)
        {
            Game game = new Game();
            Clone(source, game);
            return game;
        }
        
        public static void Clone(Game source, Game dest)
        {
            dest.game_uid = source.game_uid;
            dest.settings = source.settings;

            dest.first_player = source.first_player;
            dest.current_player = source.current_player;
            dest.turn_count = source.turn_count;
            dest.turn_timer = source.turn_timer;
            dest.state = source.state;
            dest.phase = source.phase;

            if (dest.players == null)
            {
                dest.players = new Player[source.players.Length];
                for(int i=0; i< source.players.Length; i++)
                    dest.players[i] = new Player(i);
            }

            for (int i = 0; i < source.players.Length; i++)
                Player.Clone(source.players[i], dest.players[i]);

            dest.selector = source.selector;
            dest.selector_player_id = source.selector_player_id;
            /*dest.selector_caster_uid = source.selector_caster_uid;
            dest.selector_ability_id = source.selector_ability_id;

            dest.last_destroyed = source.last_destroyed;
            dest.last_played = source.last_played;
            dest.last_target = source.last_target;
            dest.last_summoned = source.last_summoned;
            dest.ability_triggerer = source.ability_triggerer;
            dest.rolled_value = source.rolled_value;
            dest.selected_value = source.selected_value;*/

            /*CloneHash(source.ability_played, dest.ability_played);
            CloneHash(source.cards_attacked, dest.cards_attacked);*/
        }

        public static void CloneHash(HashSet<string> source, HashSet<string> dest)
        {
            dest.Clear();
            
            foreach (string str in source)
                dest.Add(str);
        }
    }
}
