using System;

namespace NagyHazi
{
    public class Program
    {
        static byte[,] Kezdet = new byte[9,9];
        static byte[,] JatekTer = new byte[9,9];

        #region Feladat 1 a, b második fele

        static void Main(string[] args)
        {
            Kezdet = Beolvas("sudoku.txt");
            JatekTer = new byte[9,9];

            Array.Copy(Kezdet, JatekTer, Kezdet.Length); // észrevétel: ha nem mátrixot hanem 2 dimenziós tömböt ( int[][] tomb (én ezt jobban szeretem) )
                                                         // próbálunk meg másoltatni akkor csak mint shallow copy müködik, és csak mátrixnál müködik úgy minta a deep copy
            GameLoop();
        }
        #endregion

        #region Feladat: 2 a, b
        // ez kicsit kókány lett
        static void GameLoop()
        {
            kiir();
            int db = 0;

            while (!WinCheck())
            {
                bool validInput = false;

                // get input
                while (!validInput)
                {
                    // sor
                    int y = 0;
                    while (y == 0)
                    {
                        Console.Write("Sor:");
                        string newY = Console.ReadLine();
                        int.TryParse(newY, out y);

                        if (y > 9 || y < 1)
                        {
                            Console.WriteLine("Rossz sor szám!");
                            y = 0;
                        }
                    }

                    // oszlop
                    int x = 0;
                    while (x == 0)
                    {
                        Console.Write("Oszlop:");
                        string newX = Console.ReadLine();
                        int.TryParse(newX, out x);

                        if (x > 9 || x < 1)
                        {
                            Console.WriteLine("Rossz oszlop szám!");
                            x = 0;
                        }
                    }

                    // Szam
                    byte szam = 0;

                    x--;
                    y--;

                    int errX = -1;
                    int errY = -1;

                    while (szam == 0)
                    {
                        Console.Write("ért:");
                        string newSzam = Console.ReadLine();

                        if (!byte.TryParse(newSzam, out szam))
                        {
                            Console.WriteLine("rossz szám");
                            break;
                        }

                        if ((szam < 1 || szam > 9))
                        {
                            szam = 0;
                            Console.WriteLine("Hibás szám!");
                            continue;
                        }

                        db++; // ha jó ha nem lépésnek számolja (nem volt egyértelmű a feladat leírásából hogy mikor számít lépésnek)
                              // mert ha ugye csak azt számolnánk lépésnek ami jó lépés akkor mindig 51 lenne tehát gondolom a rossz lépés is számít

                        if (IsValidPos(x, y, szam, out errX, out errY))
                        {
                            validInput = true;
                            JatekTer[y, x] = szam;
                            kiir();
                        }
                        else
                        {
                            kiir(errX, errY);
                        }
                    }
                }
            }

            kiir();
            Console.WriteLine($"Gratulálok nyertél ({db} lépésből)");
        }
        #endregion

        #region Feladat 3 a

        /// <summary>
        /// megadja hogy egy érték beszúrható-e az adott helyre
        /// </summary>
        /// <param name="x">x pos (érték)</param>
        /// <param name="y">y pos (érték)k</param>
        /// <param name="szam">érték</param>
        /// <param name="errX">"kiadja" az x helyét ami a hibát okozza</param>
        /// <param name="errY">"kiadja" az y helyét ami a hibát okozza</param>
        /// <returns>true ha beszúrhetó az érték / false ha nem beszúrható</returns>
        static bool IsValidPos(int x, int y, int szam, out int errX, out int errY)
        {
            errX = -1; errY = -1;
            if (Kezdet[y, x] != 0)
            {
                return false;
            }

            for (int i = 0; i < 9; i++)
            {
                //sor
                if (JatekTer[y,i] == szam && i != x)
                {
                    errX = i;
                    errY = y;
                    return false;
                }

                //oszlop
                if (JatekTer[i,x] == szam && i != y)
                {
                    errX = x;
                    errY = i;
                    return false;
                }
            }

            /* !!! végül ez nem kellett de müködik !!! (egy négyzetben sem lehet ugyan az a szám, talán nehezítésnek jó lehet)

            int sorStartIndex = x/3;
            int oszlopStartIndex = x/3;

            for (int i = sorStartIndex*3; i < sorStartIndex*3+3; i++) 
            {
                for (int j = oszlopStartIndex*3; j < oszlopStartIndex*3+3; j++)
                {
                    Debug.WriteLine($"y:{i} x:{j}, ért: {JatekTer[i, j]}");
                    if (JatekTer[i,j] == szam)
                    {
                        errX = j;
                        errY = i;
                        return false;
                    }
                }
            }
            */

            return true;
        }
        #endregion

        #region Feladat 3 b

        /// <returns>true ha vége van a játéknak / false ha még kész a pálya</returns>
        static bool WinCheck()
        {
            // (van-e kitöltetlen mező)
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (JatekTer[i, j] == 0)
                        return false;
                }
            }

            return true;
        }
        #endregion

        #region Feladat: 4

        /// <summary>
        /// alias for kiir(-1, -1)
        /// </summary>
        static void kiir()
        {
            kiir(-1, -1);
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
                Console.Write((i+1).ToString() + " | ");
                bool van = false;
                for (int j = 0; j < 9; j++)
                {
                    if (i == errY && j == errX && errY != -1 && errX != -1) // hiba hely
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        if(JatekTer[i, j] != 0 && Kezdet[i, j] == 0)
                            Console.BackgroundColor = ConsoleColor.Green;

                        Console.Write($"{JatekTer[i, j]}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor= ConsoleColor.Black;
                        Console.Write($" ");
                    }
                    else if (JatekTer[i, j] != 0 && Kezdet[i, j] == 0) // jó érték
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{JatekTer[i, j]} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Kezdet[i, j] == 0 && JatekTer[i, j] == 0) // nincs érték
                    {
                        Console.Write("  ");
                    }
                    else                                               // kezdeti érték
                    {
                        Console.ForegroundColor= ConsoleColor.Yellow;
                        Console.Write($"{Kezdet[i, j]} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if ((j+1) % 3 == 0)
                        Console.Write("| ");
                }
                Console.WriteLine("");

                if ((i + 1) % 3 == 0)
                    Console.WriteLine("  +-------+-------+-------+");
            }

            Console.WriteLine("\nSzín magyarázat: ");

            Console.ForegroundColor= ConsoleColor.Green;
            Console.Write("0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(": jól beírt érték (zöld karakter)");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(": ez az érték akadályozza az új érték behelyezését (piros karakter)");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("0");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(": ez az felhasználó által beírt érték jó, de akadályozza az új érték behelyezését (piros karakter zöld háttérrel)\n");
        }
        #endregion

        #region Feladat 1 b első fele

        static byte[,] Beolvas(string file)
        {
            StreamReader f = new StreamReader(file);
            byte[,] Tabla = new byte[9,9];

            int sorIndex = 0;
            while (!f.EndOfStream)
            {
                string[] reszek = f.ReadLine().Split(' ');
                int[] sor = new int[9];
                for (int i = 0; i < reszek.Length; i++)
                {
                    Tabla[sorIndex,i] = byte.Parse(reszek[i]);
                }

                sorIndex++;
            }

            return Tabla;
        }
        #endregion
    }
}
