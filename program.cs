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
		static int Main(string[] args)
		{
			String board = args[1];
			String player = args[3];
			int player_num, other_player;

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

			int move = 3;
			int[][] BoardArray = JsonConvert.DeserializeObject<int[][]>(board);

			Random rnd = new Random();

			int our_possible_move, their_possible_move;
			our_possible_move = their_possible_move = -1;
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

			if (our_possible_move != -1)
			{
				move = our_possible_move;
			}
			else if (their_possible_move != -1)
			{
				move = their_possible_move;
			}
			else {
				move = makeNewBoard(BoardArray, player_num, 8);
			}

			if (move == -1)
			{
				move = rnd.Next(0, 6);
			}

			while (BoardArray[0][move] > 0)
			{
				move = rnd.Next(0, 6);
			}
			return move;
		}

		/*
		 * Returns the index of a column that has three in a row of either color
		 * or -1 if none can be found.
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

					// Check for 2 then 1 in a row
					if (col != 4)
					{
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

		static int checkForRows(int[][] BoardArray, int player_num)
		{
			int verticalRowCheck, horizontalRowCheck, diagonalRowCheck;
			verticalRowCheck = horizontalRowCheck = diagonalRowCheck = -1;
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