using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers_Minimax
{
    public class CheckerBoard
    {
        enum moveType
        {
            UpLeft, 
            UpRight, 
            DownLeft, 
            DownRight, 
            UpLeftJump, 
            UpRightJump, 
            DownLeftJump, 
            DownRightJump, 
            None
        }

        public List<int[]> blackPoses { get; set; }
        public List<int[]> redPoses { get; set; }
        public List<int[]> blackKings { get; set; }
        public List<int[]> redKings { get; set; }
        public int heuristic { get; set; }
        public bool redTurn { get; set; }

        public CheckerBoard()
        {
            blackPoses = new List<int[]>();
            redPoses = new List<int[]>();
            blackKings = new List<int[]>();
            redKings = new List<int[]>();
            generateStartingBoard();
            calculateHeuristic();
            redTurn = false;
        }

        public CheckerBoard(CheckerBoard board)
        {
            this.blackPoses = new List<int[]>();
            this.redPoses = new List<int[]>();
            this.blackKings = new List<int[]>();
            this.redKings = new List<int[]>();

            for (int i = 0; i < board.blackPoses.Count; i++)
            {
                this.blackPoses.Add(new int[2] {board.blackPoses[i][0], board.blackPoses[i][1]});
            }

            for (int i = 0; i < board.redPoses.Count; i++)
            {
                this.redPoses.Add(new int[2] {board.redPoses[i][0], board.redPoses[i][1]});
            }

            for (int i = 0; i < board.blackKings.Count; i++)
            {
                this.blackKings.Add(new int[2] {board.blackKings[i][0], board.blackKings[i][1]});
            }

            for (int i = 0; i < board.redKings.Count; i++)
            {
                this.redKings.Add(new int[2] {board.redKings[i][0], board.redKings[i][1]});
            }

            this.heuristic = board.heuristic;
            this.redTurn = board.redTurn;
        }

        private void generateStartingBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    blackPoses.Add(new int[] { i, (k*2)+((i+1)%2) });
                    redPoses.Add(new int[] { 7-i, 7-((k*2)+((i+1)%2)) });
                }
            }

            heuristic = 0;
        }

        private void calculateHeuristic()
        {

            if (redPoses.Count == 0)
            {
                this.heuristic = 100;
            }
            else if (blackPoses.Count == 0)
            {
                this.heuristic = -100;
            }
            else
            {
                int heuristic = 0;

                foreach (var blackPose in blackPoses)
                {
                    foreach (var redPose in redPoses)
                    {
                        heuristic += getConflictingPieces(redPose, blackPose);
                    }
                }

                heuristic += 2 * (12 - redPoses.Count);
                heuristic -= 2 * (12 - blackPoses.Count);

                heuristic += 10 * blackKings.Count;
                heuristic -= 10 * redKings.Count;

                this.heuristic = heuristic;
            }
        }

        private int getConflictingPieces(int[] red, int[] black)
        {
            int val = 0;

            if (red[0] - black[0] == 1 && Math.Abs(red[1] - black[1]) == 1 && redTurn)
            {
                if (!redPoses.Exists(r => red[0] - (black[0] - red[0]) == r[0] && red[1] - (black[1] - red[1]) == r[1]) || 
                    !blackPoses.Exists(b => red[0] - (black[0] - red[0]) == b[0] && red[1] - (black[1] - red[1]) == b[1]))
                { 
                    val--;
                }
            }
            else if (Math.Abs(red[0] - black[0]) == 1 && Math.Abs(red[1] - black[1]) == 1 && !redTurn)
            {
                if (!redPoses.Exists(r => red[0] - (black[0] - red[0]) == r[0] && red[1] - (black[1] - red[1]) == r[1]) || 
                    !blackPoses.Exists(b => red[0] - (black[0] - red[0]) == b[0] && red[1] - (black[1] - red[1]) == b[1]))
                { 
                    val++;
                }
            }

            return val;
        }

        public List<CheckerBoard> generateNeighbors()
        {
            List<CheckerBoard> neighbors = new List<CheckerBoard>();
            var moveables = getMoves();

            for (int i = 0; i < moveables.Count; i++)
            {
                var group = getPieceGroup(this, moveables[i]);
                var moves = getValidMove(this, group, moveables[i]);

                foreach (var move in moves)
                {
                    CheckerBoard newState = performAction(moveables[i], move);
                    newState.redTurn = !newState.redTurn;
                    newState.calculateHeuristic();
                    neighbors.Add(newState);
                }
            }
            
            return neighbors;
        }

        private List<int[]> getMoves()
        {
            if (redTurn)
            {
                return redPoses.Concat(redKings).ToList();
            }
            else
            {
                return blackPoses.Concat(blackKings).ToList();
            }
        }

        private CheckerBoard performAction(int[] pose, moveType action)
        {
            CheckerBoard newState = new CheckerBoard(this);
            var group = getPieceGroup(newState, pose);
            group.Remove(group.FirstOrDefault(b => b[0] == pose[0] && b[1] == pose[1]));

            if (action == moveType.DownLeft)
            {
                if (group == newState.blackPoses)
                {
                    if (pose[0]+1 != 7)
                    {
                        group.Add(new int[2] {pose[0]+1, pose[1]-1});
                    }
                    else
                    {
                        newState.blackKings.Add(new int[2] {pose[0]+1, pose[1]-1});
                    }
                }
                else
                {
                    group.Add(new int[2] {pose[0]+1, pose[1]-1});
                }
            }
            else if (action == moveType.DownLeftJump)
            {
                performJump(newState, group, pose);
            }
            else if (action == moveType.DownRight)
            {
                if (group == newState.blackPoses)
                {
                    if (pose[0]+1 != 7)
                    {
                        group.Add(new int[2] {pose[0]+1, pose[1]+1});
                    }
                    else
                    {
                        newState.blackKings.Add(new int[2] {pose[0]+1, pose[1]+1});
                    }
                }
                else
                {
                    group.Add(new int[2] {pose[0]+1, pose[1]+1});
                }
            }
            else if (action == moveType.DownRightJump)
            {
                performJump(newState, group, pose);
            }
            else if (action == moveType.UpLeft)
            {
                if (group == newState.redPoses)
                {
                    if (pose[0]-1 != 0)
                    {
                        group.Add(new int[2] {pose[0]-1, pose[1]-1});
                    }
                    else
                    {
                        newState.redKings.Add(new int[2] {pose[0]-1, pose[1]-1});
                    }
                }
                else
                {
                    group.Add(new int[2] {pose[0]-1, pose[1]-1});
                }
            }
            else if (action == moveType.UpLeftJump)
            {
                performJump(newState, group, pose);
            }
            else if (action == moveType.UpRight)
            {
                if (group == newState.redPoses)
                {
                    if (pose[0]-1 != 0)
                    {
                        group.Add(new int[2] {pose[0]-1, pose[1]+1});
                    }
                    else
                    {
                        newState.redKings.Add(new int[2] {pose[0]-1, pose[1]+1});
                    }
                }
                else
                {
                    group.Add(new int[2] {pose[0]-1, pose[1]+1});
                }
            }
            else if (action == moveType.UpRightJump)
            {
                performJump(newState, group, pose);
            }
                
            return newState;
        }

        private static List<int[]> getPieceGroup(CheckerBoard newState, int[] pose)
        {
            if (newState.blackPoses.Exists(b => b[0] == pose[0] && b[1] == pose[1]))
            {
                return newState.blackPoses;
            }
            else if (newState.redPoses.Exists(r => r[0] == pose[0] && r[1] == pose[1]))
            {
                return newState.redPoses;
            }
            else if (newState.blackKings.Exists(b => b[0] == pose[0] && b[1] == pose[1]))
            {
                return newState.blackKings;
            }
            else
            {
                return newState.redKings;
            }
        }

        private static void performJump(CheckerBoard newState, List<int[]> group, int[] pose)
        {
            var jump = getValidMove(newState, group, pose);
            int[] currPose = pose;
                
            do
            {
                if (jump.Contains(moveType.DownLeftJump))
                {
                    group.Remove(group.FirstOrDefault(p => p[0] == currPose[0] && p[1] == currPose[1]));
                    removeEnemy(newState, new int[2] { currPose[0]+1, currPose[1]-1 });
                    group.Add(new int[2] {currPose[0]+2, currPose[1]-2});
                }
                else if (jump.Contains(moveType.DownRightJump))
                {
                    group.Remove(group.FirstOrDefault(p => p[0] == currPose[0] && p[1] == currPose[1]));
                    removeEnemy(newState, new int[2] { currPose[0]+1, currPose[1]+1 });
                    group.Add(new int[2] {currPose[0]+2, currPose[1]+2});
                }
                else if (jump.Contains(moveType.UpLeftJump))
                {
                    group.Remove(group.FirstOrDefault(p => p[0] == currPose[0] && p[1] == currPose[1]));
                    removeEnemy(newState, new int[2] { currPose[0]-1, currPose[1]-1 });
                    group.Add(new int[2] {currPose[0]-2, currPose[1]-2});
                }
                else if (jump.Contains(moveType.UpRightJump))
                {
                    group.Remove(group.FirstOrDefault(p => p[0] == currPose[0] && p[1] == currPose[1]));
                    removeEnemy(newState, new int[2] { currPose[0]-1, currPose[1]+1 });
                    group.Add(new int[2] {currPose[0]-2, currPose[1]+2});
                }

                currPose = group[^1];
                jump = getValidMove(newState, group, currPose);
            }
            while (jump.Contains(moveType.DownLeftJump) || jump.Contains(moveType.DownRightJump) || 
                   jump.Contains(moveType.UpLeftJump) || jump.Contains(moveType.UpRightJump));

            if (group == newState.blackPoses)
            {
                if (group[^1][0] == 7)
                {
                    newState.blackKings.Add(new int[2] {group[^1][0], group[^1][1]});
                    group.Remove(group[^1]);
                }
            }
            else if (group == newState.redPoses)
            {
                if (group[^1][0] == 0)
                {
                    newState.redKings.Add(new int[2] {group[^1][0], group[^1][1]});
                    group.Remove(group[^1]);
                }
            }
        }

        private static List<moveType> getValidMove(CheckerBoard newState, List<int[]> group, int[] pose)
        {
            List<moveType> moves = new List<moveType>();
            var enemyGroup = getEnemyGroup(newState);

            if (newState.redTurn || group == newState.blackKings)
            {
                if (pose[0] > 0 && pose[1] > 0 && 
                    !newState.blackPoses.Exists(bp => bp[0] == pose[0]-1 && bp[1] == pose[1]-1) && 
                    !newState.redPoses.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]-1) &&
                    !newState.redKings.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]-1) &&
                    !newState.blackKings.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]-1))
                {
                    moves.Add(moveType.UpLeft);
                }
                else if (pose[0] > 1 && pose[1] > 1 && 
                        enemyGroup.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]-1) && 
                        !newState.redPoses.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]-2) && 
                        !newState.blackPoses.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]-2) &&
                        !newState.redKings.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]-2) &&
                        !newState.blackKings.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]-2))
                {
                    moves.Add(moveType.UpLeftJump);
                }

                if (pose[0] > 0 && pose[1] < 7 && 
                    !newState.blackPoses.Exists(bp => bp[0] == pose[0]-1 && bp[1] == pose[1]+1) && 
                    !newState.redPoses.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]+1) &&
                    !newState.redKings.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]+1) &&
                    !newState.blackKings.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]+1))
                {
                    moves.Add(moveType.UpRight);
                }
                else if (pose[0] > 1 && pose[1] < 6 && 
                        enemyGroup.Exists(rp => rp[0] == pose[0]-1 && rp[1] == pose[1]+1) && 
                        !newState.redPoses.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]+2) &&
                        !newState.blackPoses.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]+2) &&
                        !newState.redKings.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]+2) &&
                        !newState.blackKings.Exists(rp => rp[0] == pose[0]-2 && rp[1] == pose[1]+2))
                {
                    moves.Add(moveType.UpRightJump);
                }
            }
            
            if (!newState.redTurn || group == newState.redKings)
            {
                if (pose[0] < 7 && pose[1] > 0 && 
                    !newState.blackPoses.Exists(bp => bp[0] == pose[0]+1 && bp[1] == pose[1]-1) && 
                    !newState.redPoses.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]-1) &&
                    !newState.redKings.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]-1) &&
                    !newState.blackKings.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]-1))
                {
                    moves.Add(moveType.DownLeft);
                }
                else if (pose[0] < 6 && pose[1] > 1 && 
                        enemyGroup.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]-1) && 
                        !newState.redPoses.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]-2) && 
                        !newState.blackPoses.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]-2) &&
                        !newState.redKings.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]-2) &&
                        !newState.blackKings.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]-2))
                {
                    moves.Add(moveType.DownLeftJump);
                }

                if (pose[0] < 7 && pose[1] < 7 && 
                    !newState.blackPoses.Exists(bp => bp[0] == pose[0]+1 && bp[1] == pose[1]+1) && 
                    !newState.redPoses.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]+1) &&
                    !newState.redKings.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]+1) &&
                    !newState.blackKings.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]+1))
                {
                    moves.Add(moveType.DownRight);
                }
                else if (pose[0] < 6 && pose[1] < 6 && 
                        enemyGroup.Exists(rp => rp[0] == pose[0]+1 && rp[1] == pose[1]+1) && 
                        !newState.redPoses.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]+2) &&
                        !newState.blackPoses.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]+2) &&
                        !newState.redKings.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]+2) &&
                        !newState.blackKings.Exists(rp => rp[0] == pose[0]+2 && rp[1] == pose[1]+2))
                {
                    moves.Add(moveType.DownRightJump);
                }
            }
        
            return moves;
        }

        private static void removeEnemy(CheckerBoard newState, int[] enemy)
        {
            if (newState.redTurn)
            {
                if (newState.blackPoses.Exists(r => r[0] == enemy[0] && r[1] == enemy[1]))
                {
                    newState.blackPoses.Remove(newState.blackPoses.FirstOrDefault(r => r[0] == enemy[0] && r[1] == enemy[1]));
                }
                else
                {
                    newState.blackKings.Remove(newState.blackKings.FirstOrDefault(r => r[0] == enemy[0] && r[1] == enemy[1]));
                }
            }
            else
            {
                if (newState.redPoses.Exists(r => r[0] == enemy[0] && r[1] == enemy[1]))
                {
                    newState.redPoses.Remove(newState.redPoses.FirstOrDefault(r => r[0] == enemy[0] && r[1] == enemy[1]));
                }
                else
                {
                    newState.redKings.Remove(newState.redKings.FirstOrDefault(r => r[0] == enemy[0] && r[1] == enemy[1]));
                }
            }
        }

        private static List<int[]> getEnemyGroup(CheckerBoard newState)
        {
            if (newState.redTurn)
            {
                return newState.blackPoses.Concat(newState.blackKings).ToList();
            }
            else
            {
                return newState.redPoses.Concat(newState.redKings).ToList();
            }
        }


        public string toString(string start)
        {
            string str = start;
            str += "Printing Board State with Heuristic of " + this.heuristic + "\n";
            str += start;

            for (int i = 0; i < 8 * 5; i++)
            {
                str += "-";
            }

            str += "\n" + start;

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (blackPoses.Exists(p => p[0] == i && p[1] == k))
                    {
                        str += "[ B ]";
                    }
                    else if (redPoses.Exists(p => p[0] == i && p[1] == k))
                    {
                        str += "[ R ]";
                    }
                    else
                    {
                        str += "[ 0 ]";
                    }
                }

                str += "\n" + start;
            }

            for (int i = 0; i < 8 * 5; i++)
            {
                str += "-";
            }

            str += "\n";
            return str;
        }
    }
}