using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;

namespace connectfour
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Current board state and player number are passed as
            // command line arguments
            private String board = args[1];
            private String player = args[3];
            private int player_num, other_player, our_possible_move, their_possible_move;
            our_possible_move = their_possible_move = -1;
            private Random rnd = new Random();
            private const int DEPTH = 8;

            if (player == "player-one")
            {
                player_num = 1;
                other_player = 2;
            }`
            else if (player == "player-two")
            {
                player_num = 2;
                other_player = 1;
            }
            else
            {
                player_num = 0;
                other_player = 0;
            }

            // Set default (random) move
            int move = 3;

            // Parse the board JSON
            int[][] BoardArray = JsonConvert.DeserializeObject<int[][]>(board);

            /**
             * In this block, check if we have a winning move,
             * and check if we need to block them anywhere.
             *
             * The try{}catch{} blocks are there so that execution
             * can continue gracefully in the unlikely event of an error
             */
            try
            {
                our_possible_move = checkForRows(BoardArray, player_num);
            }
            catch { }
            try
            {
                their_possible_move = checkForRows(BoardArray, other_player);
            }
            catch { }

            /**
             * Evaluate the possibilites.
             * Check if we have an available winning move first,
             * then, if not, check if we can block them anywhere
             *
             * Finally, if neither is possible, try to predict moves
             * that will lead to a winning move using a pseudo-Minimax
             * algorithm
             */
            if (our_possible_move != -1)
            {
                move = our_possible_move;
            }
            else if (their_possible_move != -1)
            {
                move = their_possible_move;
            }
            else {
                move = makeNewBoard(BoardArray, player_num, DEPTH);
            }

            // If, after all that, no optimal move is found, pick one at random
            if (move == -1)
            {
                move = rnd.Next(0, 6);
            }

            // Ensure that the column of the chosen move is not full
            while (BoardArray[0][move] > 0)
            {
                move = rnd.Next(0, 6);
            }

            // Make the move
            return move;
        }

        /**
         * Returns the index of a column that has three in a row,
         * with an open space on top, or -1 if none can be found.
         */
        static int checkForVerticalRows(int[][] BoardArray, int player_num)
        {
            for (int col = 0; col <= 6; col++)
            {
                for (int row = 0; row <= 3; row++)
                {
                    // Check for 3 in a row
                    if ((BoardArray[row][col] == BoardArray[row + 1][col]) &&
                        (BoardArray[row + 1][col] == BoardArray[row + 2][col]) &&
                        (BoardArray[row + 2][col] == player_num) &&
                        (BoardArray[row - 1][col] == 0))
                    {
                        return col;
                    }
                }
            }
            return -1;
        }

        /**
         * Returns the index of a column which, if a piece is placed there,
         * will result in completion of a four in a row horizontally,
         * or -1 if no possibilities are found.
         */
        static int checkForHorizontalRows(int[][] BoardArray, int player_num)
        {
            for (int row = 0; row <= 5; row++)
            {
                for (int col = 0; col <= 4; col++)
                {
                    // Check for 3 in a row
                    if ((BoardArray[row][col] == BoardArray[row][col + 1]) &&
                        (BoardArray[row][col + 1] == BoardArray[row][col + 2]) &&
                        (BoardArray[row][col + 2] == player_num))
                    {
                        if (col - 1 >= 0)
                            if ((BoardArray[row][col - 1] == 0))
                                if (row + 1 > 5)
                                {
                                    return col - 1;
                                }
                                else if (BoardArray[row + 1][col - 1] > 0)
                                {
                                    return col - 1;
                                }


                                else if (col + 3 <= 6)
                                {
                                    if ((BoardArray[row][col + 3] == 0))
                                    {
                                        if (row + 1 > 5)
                                        {
                                            return col + 3;
                                        }
                                        else if (BoardArray[row + 1][col + 3] > 0)
                                        {
                                            return col + 3;
                                        }
                                    }
                                }
                    }

                    // Check for three in a row, with gaps in the middle
                    if (col < 4)
                    {
                        // Check for 2 then 1 in a row
                        if ((BoardArray[row][col] == BoardArray[row][col + 1]) &&
                            (BoardArray[row][col + 1] == BoardArray[row][col + 3]) &&
                            (BoardArray[row][col] == player_num))
                        {
                            if (BoardArray[row][col + 2] == 0)
                            {
                                if (row + 1 > 5)
                                    return col + 2;
                                else if (BoardArray[row + 1][col + 2] > 0)
                                {
                                    return col + 2;
                                }
                            }
                        }

                        // Check for 1 then 2 in a row
                        if ((BoardArray[row][col] == BoardArray[row][col + 2]) &&
                            (BoardArray[row][col + 2] == BoardArray[row][col + 3]) &&
                            (BoardArray[row][col] == player_num))
                        {
                            if (BoardArray[row][col + 1] == 0)
                            {
                                if (row + 1 > 5)
                                {
                                    return col + 1;
                                }
                                else if (BoardArray[row + 1][col + 1] > 0)
                                {
                                    return col + 1;
                                }
                            }
                        }
                    }
                }
            }
            return -1;
        }

        /**
         * Returns the index of a column which, if a piece is placed there,
         * will result in completion of a four in a row diagonally,
         * or -1 if no possibilities are found.
         */
        static int checkForDiagonalRows(int[][] BoardArray, int player_num)
        {
            for (int col = 2; col <= 4; col++)
            {
                for (int row = 0; row <= 3; row++)
                {
                    //3 in row /
                    if ((BoardArray[row][col] == BoardArray[row + 1][col - 1]) &&
                        (BoardArray[row + 1][col - 1] == BoardArray[row + 2][col - 2]) &&
                        (BoardArray[row][col] == player_num))
                    {
                        if (BoardArray[row + 3][col - 3] == 0)
                            return col - 3;
                        if (BoardArray[row - 1][col + 1] == 0)
                            return col + 1;
                    }

                    // 2 then 1 in row /
                    if ((BoardArray[row][col] == BoardArray[row + 1][col - 1]) &&
                        (BoardArray[row + 1][col - 1] == BoardArray[row + 3][col - 3]) &&
                        (BoardArray[row][col] == player_num))
                    {
                        if (BoardArray[row + 2][col - 2] == 0)
                            return col - 2;
                    }

                    // 1 then 2 in row /
                    if ((BoardArray[row][col] == BoardArray[row + 2][col - 2]) &&
                        (BoardArray[row + 2][col - 2] == BoardArray[row + 3][col - 3]) &&
                        (BoardArray[row][col] == player_num))
                    {
                        if (BoardArray[row + 3][col - 3] == 0)
                            return col - 3;
                    }

                    // 3 in row \
                    if ((BoardArray[row][col] == BoardArray[row + 1][col + 1]) &&
                        (BoardArray[row + 1][col + 1] == BoardArray[row + 2][col + 2]) &&
                        (BoardArray[row][col] == player_num))
                    {
                        if (BoardArray[row + 3][col + 3] == 0)
                            return col + 3;
                        if (BoardArray[row - 1][col - 1] == 0)
                            return col - 1;
                    }

                    // 2 down 1 right
                    if ((BoardArray[row][col] == BoardArray[row + 1][col + 1]) &&
                        (BoardArray[row + 1][col + 1] == BoardArray[row + 3][col + 3]) &&
                        (BoardArray[row][col] == player_num))
                    {
                        if (BoardArray[row + 2][col + 2] == 0)
                            return col + 2;
                    }

                    //1  down 2 right
                    if ((BoardArray[row][col] == BoardArray[row + 2][col + 2]) &&
                        (BoardArray[row + 2][col + 2] == BoardArray[row + 3][col + 3]) &&
                        (BoardArray[row][col] == player_num))
                    {
                        if (BoardArray[row + 1][col + 1] == 0)
                            return col + 1;
                    }
                }
            }
            return -1;
        }

        /**
         * Returns the index of any move that can be made that will result in
         * the completion of four in a row in any direction for the given player
         * or -1 if none can be found.
         */
        static int checkForRows(int[][] BoardArray, int player_num)
        {
            int verticalRowCheck, horizontalRowCheck, diagonalRowCheck;
            verticalRowCheck = horizontalRowCheck = diagonalRowCheck = -1;

            // Use try{}catch{} blocks to continue gracefully in the unlikely event of an error
            try { verticalRowCheck = checkForVerticalRows(BoardArray, player_num); } catch { }
            try { horizontalRowCheck = checkForHorizontalRows(BoardArray, player_num); } catch { }
            try { diagonalRowCheck = checkForDiagonalRows(BoardArray, player_num); } catch { }

            if (verticalRowCheck != -1)
                return verticalRowCheck;
            else if (horizontalRowCheck != -1)
                return horizontalRowCheck;
            else if (diagonalRowCheck != -1)
                return diagonalRowCheck;
            else
                return -1;
        }

        /**
         * Returns the index of a move that would result in a winning board for
         * the given player in less than DEPTH turns.
         *
         * Does this by first producing an array of 7 new boards, one representing
         * each possible move at this point, then checking if any of them result in
         * a possible win.
         * If not, repeat recursively using DEPTH-1 each time so
         * the recursion can terminate. In this case, return the index of the
         * "parents" of the winning board so that the top level makes the appropriate
         * move.
         *
         * If no solution is found up to the given depth, return -1.
         */
        static int makeNewBoard(int[][] BoardArray, int player_num, int depth)
        {
            if (depth <= 0)
            {
                return -1;
            }

            ArrayList ArrayofBoards = new ArrayList();

            for (int col = 0; col <= 6; col++)
            {
                for (int row = 5; row >= 0; row--)
                {
                    if (BoardArray[row][col] == 0)
                    {
                        BoardArray[row][col] = player_num;
                        ArrayofBoards.Add(BoardArray);
                        BoardArray[row][col] = 0;
                    }
                }
            }

            int count = -1;
            foreach (int[][] board in ArrayofBoards)
            {
                count++;
                if (checkForRows(board, player_num) != -1)
                {
                    return checkForRows(board, player_num);
                }
                else if (makeNewBoard(board, player_num % 2 + 1, depth - 1) != -1)
                {
                    return count;
                }
                else {
                    return -1;
                }
            }
            return -1;
        }
    }
}