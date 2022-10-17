using System;

namespace Tennis
{
    public class TennisGame1 : ITennisGame
    {
        private readonly Player player1;
        private readonly Player player2;
        private readonly Referee referee;

        public TennisGame1(string player1Name, string player2Name)
        {
            player1 = new Player(player1Name);
            player2 = new Player(player2Name);
            referee = new Referee();
        }

        public void WonPoint(string playerName)
        {
            if (playerName == "player1")
            {
                player1.Scores();
            }
            else
            {
                player2.Scores();
            }
        }

        public string GetScore()
        {
            return referee.Determine(player1, player2).Score();
        }
    }

    public class Player
    {
        public Player(string name)
        {
            this.name = name;
            Score = 0;
        }

        private readonly string name;

        public int Score
        {
            get;
            private set;
        }

        public void Scores()
        {
            Score++;
        }

        public bool Draw(Player player)
        {
            return Score == player.Score;
        }
        
        public bool PlayerHasAdvantage(Player player)
        {
            return Score >= 4 || player.Score >= 4;
        }

        public bool TieBreak(Player player)
        {
            return 
                PlayerHasAdvantage(player) && 
                PlayerIsAheadByOnePoint(player);
        }
        
        private bool PlayerIsAheadByOnePoint(Player player)
        {
            return Math.Abs(Score - player.Score) == 1;
        }
    }

    public class Referee
    {
        public Scoring Determine(Player player1, Player player2)
        {
            if (player1.Draw(player2))
            {
                return new DrawScoring(player1, player2);
            }

            if (player1.TieBreak(player2))
            {
                return new TieBreakScoring(player1, player2);
            }

            if (player1.PlayerHasAdvantage(player2))
            {
                return new WinScoring(player1, player2);
            }

            return new GeneralScore(player1, player2);
        }
    }

    public abstract class Scoring
    {
        public abstract string Score();
    }
    
    public class DrawScoring : Scoring
    {
        private readonly Player player1;

        public DrawScoring(Player player1, Player player2)
        {
            this.player1 = player1;
        }

        public override string Score()
        {
            return player1.Score switch
            {
                0 => "Love-All",
                1 => "Fifteen-All",
                2 => "Thirty-All",
                _ => "Deuce"
            };
        }
    }

    public class TieBreakScoring : Scoring
    {
        private readonly Player player1;
        private readonly Player player2;

        public TieBreakScoring(Player player1, Player player2)
        {
            this.player2 = player2;
            this.player1 = player1;
        }

        public override string Score()
        {
            return player1.Score > player2.Score
                ? "Advantage player1"
                : "Advantage player2";
        }
    }
    
    public class WinScoring : Scoring
    {
        private readonly Player player1;
        private readonly Player player2;

        public WinScoring(Player player1, Player player2)
        {
            this.player2 = player2;
            this.player1 = player1;
        }

        public override string Score()
        {
            return player1.Score > player2.Score
                ? "Win for player1"
                : "Win for player2";
        }
    }
    
    public class GeneralScore : Scoring
    {
        private readonly Player player1;
        private readonly Player player2;

        public GeneralScore(Player player1, Player player2)
        {
            this.player2 = player2;
            this.player1 = player1;
        }

        public override string Score()
        {
            return $"{RunningScore(player1.Score)}-{RunningScore(player2.Score)}";
        }
        
        private string RunningScore(int score)
        {
            return score switch
            {
                0 => "Love",
                1 => "Fifteen",
                2 => "Thirty",
                _ => "Forty"
            };
        }
    }
}


