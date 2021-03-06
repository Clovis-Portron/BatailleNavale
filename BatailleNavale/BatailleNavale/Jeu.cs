﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BatailleNavale
{
    /// <summary>
    /// Classe statique permettant de réaliser des actions en rapport au déroulement du jeu dans son ensemble
    /// </summary>
    class Jeu
    {
        /// <summary>
        /// ENumération des différents niveaux de difficulté disponibles
        /// </summary>
        public enum Niveau
        {
            ENFANT = 0,
            FACILE = 1, 
            NORMAL = 2
        };

        /// <summary>
        /// Niveau de jeu actuel (Facile par défaut)
        /// </summary>
        public static Niveau NiveauJeu = Niveau.FACILE;

        /// <summary>
        /// Affichage et gestion de l'interaction du joueur du menu principal
        /// </summary>
        public static void MenuPrincipal()
        {
            Console.Clear();
            Console.SetWindowSize(90, 50);
            Console.WriteLine("======= Bataille Navale =======");
            Console.WriteLine("Version 1.0 dévelopée par Hugo Le Tarnec et Clovis Portron");
            ConsoleKey key = default(ConsoleKey);
            do
            {
                if (key != default(ConsoleKey))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Veuillez choisir une option ci-dessous");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("(N)ouvelle partie | (C)arger une partie sauvegardée | (Q)uitter");
                Console.ResetColor();
                key = Console.ReadKey(false).Key;
            }
            while (key != ConsoleKey.N && key != ConsoleKey.C && key != ConsoleKey.Q);
            Program.ViderTampon();
            if (key == ConsoleKey.N)
                Jeu.MenuNouvellePartie();
            else if (key == ConsoleKey.C)
                Jeu.MenuChargerPartie();
            else if (key == ConsoleKey.Q)
                Jeu.Quitter();
        }

        /// <summary>
        /// Affichage et gestion de l'interaction du joueur du menu de lancement d'une nouvelle partie
        /// </summary>
        public static void MenuNouvellePartie()
        {
            Console.Clear();
            Console.WriteLine("======= Nouvelle partie =======");
            ConsoleKey key = default(ConsoleKey);
            do
            {
                if (key != default(ConsoleKey))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Veuillez choisir une option ci-dessous");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("(E) Commencer une partie contre l'ordinateur niveau Enfant \n(F) Commencer une partie contre l'ordinateur niveau Facile \n(N) Commencer une partie contre l'ordinateur niveau Normal \n(R)etour");
                Console.ResetColor();
                key = Console.ReadKey().Key;
            }
            while (key != ConsoleKey.F && key != ConsoleKey.N && key != ConsoleKey.R && key != ConsoleKey.E);
            Program.ViderTampon();
            if (key == ConsoleKey.R)
                Jeu.MenuPrincipal();
            else if(key == ConsoleKey.F)
            {
                Jeu.NiveauJeu = Niveau.FACILE;
                Jeu.MenuDemarrerNouvellePartie();
            }
            else if(key == ConsoleKey.N)
            {
                Jeu.NiveauJeu = Niveau.NORMAL;
                Jeu.MenuDemarrerNouvellePartie();
            }
            else if (key == ConsoleKey.E)
            {
                Jeu.NiveauJeu = Niveau.ENFANT;
                Jeu.MenuDemarrerNouvellePartie();
            }
        }

        /// <summary>
        /// Affichage et gestion de l'interaction du joueur du menu de chargement d'une partie sauvegardée
        /// </summary>
        public static void MenuChargerPartie()
        {
            Console.Clear();
            Joueur.Start();
            Console.WriteLine("======= Charger une partie =======");
            string[] sauvegardes = Sauvegarde.RecupererFichiersSauvegarde();
            if(sauvegardes.Length <= 0)
            {
                Console.WriteLine("Aucune partie sauvegardée n'a été trouvée.");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                Jeu.MenuPrincipal();
                return;
            }
            Console.WriteLine("Veuillez sélectionner une sauvegarde ci-dessous");
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < sauvegardes.Length; i++)
            {
                Console.WriteLine("" + (i + 1) + ")" + sauvegardes[i]);
            }
            Console.ResetColor();
            int index = -1;
            do
            {
                try
                {
                    index = Convert.ToInt32(Console.ReadLine());
                    index = index - 1;
                    if (index < 0 || index >= sauvegardes.Length)
                        index = -1;
                }
                catch(Exception)
                {
                    index = -1;
                }
                if(index == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ce n'est pas une sélection valide. Veuillez réessayer.");
                    Console.ResetColor();
                }
            }
            while (index == -1);

            Sauvegarde.ReglerFichierSauvegarde(sauvegardes[index]);

            Console.Clear();
            Console.WriteLine("======= Charger une partie =======");
            try
            {
                Sauvegarde.Charger();
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Impossible de charger la partie. Etes-vous sûr d'avoir un fichier de sauvegarde existant ?");
                Console.ResetColor();
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                Jeu.MenuPrincipal();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("La partie a été chargée !");
            Console.ResetColor();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey(false);
            Jeu.DeroulementPartie();
        }


        /// <summary>
        /// Affiche et gère l'écran de paramétrage d'une nouvelle partie
        /// </summary>
        public static void MenuDemarrerNouvellePartie()
        {
            
            Console.Clear();
            Joueur.Start();
            // On met à jour l'index de sauvegarde pour éviter d'écraser une ancienne partie
            Sauvegarde.NomFichierIndex = Sauvegarde.RecupererDernierIndexSauvegarde() + 1;


            // Initialisation des grilles
            Grille.GrilleJ1 = new int[Grille.LargeurGrille, Grille.HauteurGrille];
            Grille.GrilleDecouverteJ1 = new int[Grille.LargeurGrille, Grille.HauteurGrille];

            Grille.GrilleJ2 = new int[Grille.LargeurGrille, Grille.HauteurGrille];
            Grille.GrilleDecouverteJ2 = new int[Grille.LargeurGrille, Grille.HauteurGrille];

            // Initialisation des données des bateaux des joueurs 
            Bateau.PositionBateauxJ1 = new int[Bateau.NombreTypesBateaux, 4];
            Bateau.VieBateauxJ1 = new int[Bateau.NombreTypesBateaux];

            Bateau.PositionBateauxJ2 = new int[Bateau.NombreTypesBateaux, 4];
            Bateau.VieBateauxJ2 = new int[Bateau.NombreTypesBateaux];

            Bateau.PlacerBateauxAuHasard(1);
            Bateau.PlacerBateauxAuHasard(2);

            //TODO: modifier ce code si on souhaite ajouter un mode JCJ
            Console.WriteLine("======= Nouvelle partie =======");
            Grille.AfficherGrille(Grille.GrilleJ1);
            Console.WriteLine("-------------------------------");
            ConsoleKey key = default(ConsoleKey);
            do
            {
                if (key != default(ConsoleKey))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Etes-vous satisfait de ce placement ?");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("(O)ui | N(on)");
                Console.ResetColor();
                key = Console.ReadKey(false).Key;
            }
            while (key != ConsoleKey.O && key != ConsoleKey.N);
            Program.ViderTampon();
            if (key == ConsoleKey.O)
                Jeu.DeroulementPartie();
            else
                Jeu.MenuDemarrerNouvellePartie();

        }

        /// <summary>
        /// Affiche et gère le déroulement d'une partie 
        /// </summary>
        public static void DeroulementPartie()
        {
            int joueur = 1;
            IA.Reset();
            while (true)
            {
                Console.Clear();
                Joueur.Jouer(joueur);
                if (Joueur.aPerdu(Joueur.ObtenirAutreJoueur(joueur)) == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Le joueur " + joueur + " a gagné !!!");
                    Console.ResetColor();
                    Console.WriteLine("-- Appuyez sur une touche pour continuer --");
                    Console.ReadKey(false);
                    Jeu.MenuPrincipal();
                }
                joueur = Joueur.ObtenirAutreJoueur(joueur);
                Console.WriteLine("-- Appuyez sur une touche pour passer au tour de l'autre joueur --");
                Console.ReadKey(false);
                Console.Clear();
                Joueur.Jouer(joueur);
                if (Joueur.aPerdu(Joueur.ObtenirAutreJoueur(joueur)) == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Le joueur " + joueur + " a gagné :'(");
                    Console.ResetColor();
                    Console.WriteLine("-- Appuyez sur une touche pour continuer --");
                    Console.ReadKey(false);
                    Jeu.MenuPrincipal();

                }
                joueur = Joueur.ObtenirAutreJoueur(joueur);
                if (Joueur.DemanderContinuer() == false)
                {
                    Sauvegarde.Sauvegarder();
                    Jeu.MenuPrincipal();
                }
            }
        }

        /// <summary>
        /// Affiche et gère le menu permettant de quitter le jeu
        /// </summary>
        public static void Quitter()
        {
            Console.WriteLine("Au revoir !");
            Console.Write("appuyer sur une touche");
            Console.ReadKey(false);
            System.Environment.Exit(0);
        }

    }
}
