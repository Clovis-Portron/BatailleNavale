﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatailleNavale
{
    class Grille
    {

        public const int LargeurGrille = 10;
        public const int HauteurGrille = 10;


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
        public static void remplirgrille(int[,]grille,int [,]positionbateau)
        {
           
            for (int j = 0; j < (Bateau.NombreTypesBateaux-1); j++)
            {
                    int x1 = positionbateau[j, 0];
                    int y1 = positionbateau[j, 1];
                    int x2 = positionbateau[j, 2];
                    int y2 = positionbateau[j, 3];
                    if (x1==x2)
                    {
                        for (int i=y1; i<=y2;i++)
                        {
                            grille[x1, i] = 1;
                        }
                    }
                    if(y1==y2)
                    {
                        
                        for (int i=x1; i<=x2;i++)
                        {
                            grille[i, y1] = 1;
                        }
                    }
            }
            for (int i=0;i<10;i++)
            {
                for (int j=0;j<10;j++)
                {
                    Console.Write(grille[i, j]);
                }
                Console.WriteLine(" ");
            }


        }

    }
}
