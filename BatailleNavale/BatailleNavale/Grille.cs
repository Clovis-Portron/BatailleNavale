﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatailleNavale
{
    class Grille
    {
        public static void AfficherGrille(int[,] grille)
        {
            Console.WriteLine("     A  B  C  D  E  F  G  H  I  J");
            for (int i = 0; i < 10; i++)
            {
                Console.Write("   ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write("+--");
                }
                Console.WriteLine("+");
                if (i < 9)
                {
                    Console.Write((i + 1) + "  |");
                }
                else
                {
                    Console.Write((i + 1) + " |");
                }
                for (int j = 0; j < 10; j++)
                {

                    Console.Write(grille[i, j]);
                    if (j != 9)
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine(" |");
            }
            Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+");

        }

    }
}