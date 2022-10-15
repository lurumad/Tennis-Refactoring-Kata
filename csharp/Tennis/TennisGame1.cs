using System;

namespace Tennis
{
    public class TennisGame1 : ITennisGame
    {
        private readonly Player player1;
        private readonly Player player2;
        private readonly string player1Name;
        private readonly string player2Name;
        private readonly Referee referee;

        public TennisGame1(string player1Name, string player2Name)
        {
            this.player1Name = player1Name;
            player1 = new Player(player1Name);
            this.player2Name = player2Name;
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
    }

    public class Referee
    {
        public Scoring Determine(Player player1, Player player2)
        {
            if (Draw(player1.Score, player2.Score))
            {
                return new DrawScoring(player1.Score, player2.Score);
            }

            if (TieBreak(player1.Score, player2.Score))
            {
                return new TieBreakScoring(player1.Score, player2.Score);
            }

            if (PlayerHasAdvantage(player1.Score, player2.Score))
            {
                return new WinScoring(player1.Score, player2.Score);
            }

            return new GeneralScore(player1.Score, player2.Score);
        }
        
        private bool Draw(int player1Score, int player2Score)
        {
            return player1Score == player2Score;
        }
        
        private bool TieBreak(int player1Score, int player2Score)
        {
            return PlayerHasAdvantage(player1Score, player2Score) 
                && PlayerIsAheadByOnePoint(player1Score, player2Score);
        }

        private bool PlayerHasAdvantage(int player1Score, int player2Score)
        {
            return player1Score >= 4 || player2Score >= 4;
        }

        private bool PlayerIsAheadByOnePoint(int player1Score, int player2Score)
        {
            return Math.Abs(player1Score - player2Score) == 1;
        }
    }

    public abstract class Scoring
    {
        protected Scoring(int player1Score, int player2Score)
        {
            this.player1Score = player1Score;
            this.player2Score = player2Score;
        }

        protected readonly int player1Score;
        protected readonly int player2Score;
        
        public abstract string Score();
    }
    
    public class DrawScoring : Scoring
    {
        public DrawScoring(int player1Score, int player2Score) 
            : base(player1Score, player2Score)
        {
        }

        public override string Score()
        {
            return player1Score switch
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
        public TieBreakScoring(int player1Score, int player2Score) 
            : base(player1Score, player2Score)
        {
        }

        public override string Score()
        {
            return player1Score > player2Score
                ? "Advantage player1"
                : "Advantage player2";
        }
    }
    
    public class WinScoring : Scoring
    {
        public WinScoring(int player1Score, int player2Score) 
            : base(player1Score, player2Score)
        {
        }

        public override string Score()
        {
            return player1Score > player2Score
                ? "Win for player1"
                : "Win for player2";
        }
    }
    
    public class GeneralScore : Scoring
    {
        public GeneralScore(int player1Score, int player2Score) 
            : base(player1Score, player2Score)
        {
        }

        public override string Score()
        {
            return $"{RunningScore(player1Score)}-{RunningScore(player2Score)}";
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


