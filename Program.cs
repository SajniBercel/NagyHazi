using System;
using System.Diagnostics;
using System.IO;

namespace NagyHazi
{
    public class Program
    {
        static int[][] Kezdet = new int[9][];
        static int[][] JatekTer = new int[9][];
        static void Main(string[] args)
        {
            Kezdet = Beolvas("sudoku.txt");
            JatekTer = new int[9][];

            Array.Copy(Kezdet, JatekTer, Kezdet.Length);

            GameLoop();
        }

        static void GameLoop()
        {
            kiir(-1, -1);

            while (WinCheck())
            {
                // Input
                int x = 0;
                int y = 0;

                int errX = -1;
                int errY = -1;

                int szam = 0;

                bool validInput = false;
                while (!validInput)
                {
                    errX = -1;
                    errY = -1;
                    x = 0;
                    y = 0;
                    while (y < 1)
                    {
                        Console.Write("Sor:");
                        string newY = Console.ReadLine();
                        int.TryParse(newY, out y);

                        if (y > 9 || y < 1)
                            y = 0;
                    }
                    while (x < 1)
                    {
                        Console.Write("Oszlop:");
                        string newX = Console.ReadLine();
                        int.TryParse(newX, out x);

                        if (x > 9 || x < 1)
                            x = 0;
                    }

                    //validation

                    Console.Write("ért:");
                    string newSzam = Console.ReadLine();
                    szam = 0;
                    if (int.TryParse(newSzam, out szam))
                    {
                        x--;
                        y--;

                        if (IsValidPos(x, y, szam, out errX, out errY))
                        {
                            validInput = true;

                            JatekTer[y][x] = szam;

                            Debug.WriteLine("MOST FIX y:" + y +" x: " + x + ", szam" + szam);

                            kiir(errX, errY);
                        }
                        else
                        {
                            kiir(errX, errY);
                        }
                    }
                }
            }
            
            Console.WriteLine("Gratulálok nyertél");
        }

        static bool IsValidPos(int x, int y, int szam, out int errX, out int errY)
        {
            errX = -1; errY = -1;
            if (Kezdet[y][x] != 0)
                return false;

            for (int i = 0; i < 9; i++)
            {
                //sor
                if (JatekTer[y][i] == szam && i != x)
                {
                    errX = i;
                    errY = y;
                    return false;
                }

                //oszlop
                if (JatekTer[i][x] == szam && i != y)
                {
                    errX = i;
                    errY = y;
                    return false;
                }
            }

            return true;
        }

        static bool WinCheck()
        {
            //Sor
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    
                }
            }

            //Oszlop
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    
                }
            }

            return true;
        }

        /// <param name="errX">ha nincs error akkor legyen -1</param>
        /// <param name="errY">ha nincs error akkor legyen -1</param>
        static void kiir(int errX, int errY)
        {
            Console.Clear();

            Console.WriteLine("         S U D O K U");
            Console.WriteLine("    1 2 3   4 5 6   7 8 9");
            Console.WriteLine("  +-------+-------+-------+");

            for (int i = 0; i < 9; i++)
            {
                Debug.WriteLine("Új sor");
                Console.Write((i+1).ToString() + " | ");
                bool van = false;
                for (int j = 0; j < 9; j++)
                {
                    Debug.Write((JatekTer[i][j] != 0 && Kezdet[i][j] == 0).ToString() +"   ");

                    Debug.Write(" I:" + i +", J:" + j + " Jatek: " + JatekTer[i][j] + " Kezdet: " + Kezdet[i][j]);

                    if (i == errY && j == errX && errY != -1 && errX != -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{Kezdet[i][j]} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (JatekTer[i][j] != 0 && Kezdet[i][j] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{JatekTer[i][j]} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Kezdet[i][j] == 0 && JatekTer[i][j] == 0)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write($"{Kezdet[i][j]} ");
                    }

                    if ((j+1) % 3 == 0)
                        Console.Write("| ");
                    Debug.WriteLine("");
                }
                Console.WriteLine("");

                if ((i + 1) % 3 == 0)
                    Console.WriteLine("  +-------+-------+-------+");
            }
        }

        static int[][] Beolvas(string file)
        {
            StreamReader f = new StreamReader(file);
            int[][] Tabla = new int[9][];

            int sorIndex = 0;
            while (!f.EndOfStream)
            {
                Tabla[sorIndex] = new int[9];

                string[] reszek = f.ReadLine().Split(' ');
                int[] sor = new int[9];
                for (int i = 0; i < reszek.Length; i++)
                {
                    sor[i] = int.Parse(reszek[i]);
                }

                Tabla[sorIndex] = sor;

                sorIndex++;
            }

            return Tabla;
        }
    }
}
