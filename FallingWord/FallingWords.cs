/*
 * 
 * Le but du jeu est de trouver un mot choisi de manière aléatoire parmi un ensemble de mots.
 * Le joueur dispose d'un nombre maximal de tentatives pour trouver le mot cherché.  Chaque fois
 * qu'il réussit à trouver une lettre, toutes les occurrences de cette lettre sont remplacées dans 
 * le mot caché et le mot caché demeure au même endroit.
 * 
 * Si la lettre choisie n'est pas dans le mot caché, le mot "descend" dans l'écran jusqu'à ce qu'il
 * atteigne le bas de l'écran de jeu.  Si le joueur "attrape" le mot (le découvre) avant qu'il n'atteigne
 * le bas, le joueur gagne un point.  Si au contraire, le mot tombe jusqu'en bas, le joueur perd une vie.
 * 
 * Le joueur dispose d'un nombre maximal de vie au début de la partie.
 *  
 * Auteur: Professeurs
 */
using System;
namespace FallingWordsW10
{

  class Program
  {

    static void Main ( string[] args )
    {
      FallingWords app = new FallingWords ();
      app.Run ();
    }
  }

  public partial class FallingWords
    {
        #region Constantes
        /// <summary>
        /// Largeur de la zone de jeu.
        /// </summary>
        const int LARGEUR_JEU = 80;

        /// <summary>
        /// Hauteur de la zone de jeu.
        /// </summary>
        const int HAUTEUR_JEU = 40;

        /// <summary>
        /// Nombre de "vies" dans le jeu.
        /// </summary>
        public const int NB_MOTS_MAX_POUVANT_ETRE_RATES_DANS_PARTIE = 3;

        /// <summary>
        /// Titre du jeu affiché dans la barre de titre de l'application.
        /// </summary>
        const string TITRE_JEU = "Falling Words";


        /// <summary>
        /// Position de départ verticale pour l'affichage des mots qui descendent
        /// </summary>
        const int POSITION_VERTICALE_DEBUT_JEU = 7;

        /// <summary>
        /// Position de départ verticale pour l'affichage des statistiques du jeu.
        /// </summary>
        const int POSITION_VERTICALE_DEBUT_STATISTIQUES = HAUTEUR_JEU - 9;

        const int POSITION_VERTICALE_PIED_DE_PAGE = POSITION_VERTICALE_DEBUT_STATISTIQUES + 4;

        /// <summary>
        /// "Vitesse" de descente des mots.  Comme les dimensions de la zone d'affichage du jeu ne
        /// varient pas, la vitesse doit dépendre de la taille de cette zone et du nombre maximal d'erreurs
        /// tolérées dans un mot.
        /// </summary>
        const int VITESSE_VERTICALE = (POSITION_VERTICALE_DEBUT_STATISTIQUES - POSITION_VERTICALE_DEBUT_JEU) / NB_MAX_ERREURS_DANS_MOT;

        /// <summary>
        /// Si le nombre d'erreurs excède le maximum (pour un échec sur le mot)
        /// </summary>
        public const int ECHEC_JOUEUR = -1;

        /// <summary>
        /// Si le joueur a trouvé toutes les lettres du mot (pour une "victoire" sur le mot)
        /// </summary>
        public const int SUCCES_JOUEUR = 1;

        /// <summary>
        /// Nombre maximal d'erreurs tolérées dans un mot.
        /// </summary>
        public const int NB_MAX_ERREURS_DANS_MOT = 4;

        /// <summary>
        /// Nom du programmeur du jeu
        /// </summary>
        public const string NOM_PROGRAMMEUR = "Pierre Poulin";

        /// <summary>
        /// Si ce n'est ni un échec, ni un succès (donc le joueur peut continuer à jouer ce mot)
        /// </summary>
        public const int JOUEUR_DOIT_CONTINUER = 0;
        #endregion

        /// <summary>
        /// La méthode Run est le point de départ (point d'entrée)
        /// dans notre application.
        /// </summary>
        public void Run()
        {
            // Préciser les dimensions de la fenêtre de jeu. Une seule fois, au début du programme.
            Console.WindowWidth = LARGEUR_JEU;
            Console.WindowHeight = HAUTEUR_JEU;
            Console.Title = TITRE_JEU;


            // Boucle principale du jeu
            bool veutQuitter = false;
            // Tant que le joueur ne veut pas quitter
            do
            {
                // Affichage du menu principal
                AfficherEcranPrincipal();

                // Demander au joueur ce qu'il souhaite faire.
                veutQuitter = DemanderSiJoueurVeutQuitter();

                if (false == veutQuitter)
                {
                    // Démarrer une partie
                    JouerUnePartie();
                }
            } while (false == veutQuitter);

            Console.ResetColor();
            Console.Clear();
        }

        #region Fonctions d'affichage
        /// <summary>
        /// Cette méthode est reponsable de l'affichage du menu principal du jeu
        /// Elle affiche les informations pertinentes:
        /// - Titre du jeu
        /// - Noms des auteurs
        /// - Date de réalisation
        /// - Contexte de réalisation
        /// </summary>
        void AfficherEcranPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            string title = @"                                                            
          ;:@@@@@@@@@@.                                                            
     @@@@@@@@@@@@@@@@@,                `#@@    :@@.                                
   .@@@@@:.        .@+                '@.':  .@',+`    '@                          
  `@@@@#.         .@;                @# '+  '@..@     +@+                          
                 .@'       ,@@@#`  `#+ ':  ;@`,+`                 ;       +@@'     
                +@.     +@@@@;+'   @@.#   '@.+,    @@@.   :@@: :@@@;   @@@@, .@    
         .+@@@@@@@@: `+@@+  :@@,  +@@;   .@@#    ,@@@    #@+ '@'##`  @@#`  .@@#    
       @@@@@@@@@@#` `@@,   @@+`  `#@.    +@'    :@@`   `+@.,@:.@,  .@@.  `#@@+`    
           .@#`      @# .@#.@#`.@:#@:  #;;@+` '#.@@, ,@'+@@+ .@#`'@.#+`:@#.#@.@,   
         `#@.        .@@+  ;@@@,  '@@@#  .@@@@.  ,@@@;  ;@.  '@@@   `@@+  +@@      
      +@@@.                                                             '@#        
    +@@+`                                                             +@@'         
    '                                                               `@@#.          
                                                                   `#@.            
                                                                                    
     '+@@@@@@@#                                           ''                       
  ,@@@@@@@@@@@@    .@@#  .#+                             @@'                       
  ,@@@'     '@+  `+@@+`  @@;                           .@@.                        
  ,       `@@'  ;@@@'   ,@#           .               .@@`      +@.                
         @@#. `@@@@;   .@#`  +@@@+  ;@@@@@@+     :@@@@@#`      @@@,                
       +@@:  @@.@@.   +@@  @@@:.@@..@@@@+#@`  ,@@@@: @#      #'.@@+                
     .@@#  @@,,@@    @@+ .@#. `@@#` +. .@'   @@+`  :@+`    +'  `#@.                
    +@@. @@+ `#@.  #@@,  #@,  :@@ ++  ,@:  ,@@+  #@.@'   +@@@  ++  #.              
   `+@@@@+   .@@@@@@#    .@#,@@+      +@@@@  @@@@. ,@@@@+ ;@@@@@@#.                
    `@@;      ;@@@,        .'          :.     ;    .@@:      ';";
            Console.WriteLine(title);
            Console.ResetColor();

            AfficherPiedDePage();
        }

        void AfficherPiedDePage()
        {
            Console.SetCursorPosition(0, POSITION_VERTICALE_PIED_DE_PAGE);
            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < LARGEUR_JEU; i++)
                Console.Write("+");
            // Affichage de l'auteur
            Console.WriteLine("");
            Console.WriteLine("Jeu réalisé par: {0}", FallingWords.ObtenirNomProgrammeur());
            // Affichage des infos de réalisation
            Console.WriteLine("");
            Console.WriteLine("Dans le cadre du cours Introduction à la programmation");
            for (int i = 0; i < LARGEUR_JEU; i++)
                Console.Write("+");
            Console.ResetColor();
        }

        /// <summary>
        /// Demande au joueur ce qu'il souhaite faire comme prochaine action: jouer ou quitter
        /// </summary>
        /// <returns>true si le joueur veut quitter, false s'il veut plutôt jouer</returns>
        bool DemanderSiJoueurVeutQuitter()
        {
            Console.SetCursorPosition(0, POSITION_VERTICALE_PIED_DE_PAGE - 5);
            bool joueurVeutQuitterJeu = false;

            Console.Write("Que voulez-vous faire? (Q pour quitter, autre touche pour jouer): ");
            string choixJoueur = Console.ReadLine();
            if (choixJoueur.ToUpper() == "Q")
            {
                joueurVeutQuitterJeu = false;
            }

            return joueurVeutQuitterJeu;
        }

        /// <summary>
        /// Affiche les statistiques courantes dans la partie: nombre de mots trouvés, nombre de mots ratés
        /// et nombre de tentatives restantes.
        /// </summary>
        /// <param name="nbMotsRatesDansPartie">Le nombre de mots ratés à date</param>
        /// <param name="nbMotsTrouvesDansPartie">Le nombre de mots trouvés à date</param>
        void AfficherStatistiquesPartie(int nbMotsRatesDansPartie, int nbMotsTrouvesDansPartie)
        {
            Console.SetCursorPosition(0, POSITION_VERTICALE_DEBUT_STATISTIQUES);
            Console.WriteLine("Nombre de mots trouvés dans la partie: " + nbMotsTrouvesDansPartie);
            Console.WriteLine("Nombre de mots ratés dans la partie: " + nbMotsRatesDansPartie);
            Console.ResetColor();
            Console.Write("Il vous reste ");
            if (nbMotsRatesDansPartie == (NB_MOTS_MAX_POUVANT_ETRE_RATES_DANS_PARTIE - 1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (nbMotsRatesDansPartie > (NB_MOTS_MAX_POUVANT_ETRE_RATES_DANS_PARTIE / 2))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write(NB_MOTS_MAX_POUVANT_ETRE_RATES_DANS_PARTIE - nbMotsRatesDansPartie);
            Console.ResetColor();
            Console.Write(" chances dans cette partie");
        }

        /// <summary>
        /// Affiche l'entête du jeu en mode partie.
        /// </summary>
        void AfficherEnteteJeu()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < LARGEUR_JEU; i++)
            {
                Console.Write("+");
            }
            for (int j = 1; j <= 3; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("+");
                }

                for (int i = LARGEUR_JEU - 5; i < LARGEUR_JEU; i++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("+");
                }
            }
            // Écrire le titre du jeu centré dans l'écran
            int positionDebutTitre = (LARGEUR_JEU - TITRE_JEU.Length) / 2;
            Console.SetCursorPosition(positionDebutTitre, 2);
            Console.Write(TITRE_JEU);

            Console.SetCursorPosition(0, 4);
            for (int i = 0; i < LARGEUR_JEU; i++)
            {
                Console.Write("+");
            }
        }

        /// <summary>
        /// Efface la section réservée au jeu pendant un tour de jeu (lorsque
        /// le joueur tente de découvrir un mot).
        /// </summary>
        void EffacerSectionJeu()
        {
            for (int j = POSITION_VERTICALE_DEBUT_JEU; j < POSITION_VERTICALE_DEBUT_STATISTIQUES; j++)
            {
                Console.SetCursorPosition(0, j);
                for (int i = 0; i < Console.WindowWidth; i++)
                    Console.Write(" ");
            }
        }
        #endregion

        #region Fin de mots et/ou de partie

        /// <summary>
        /// Joue une "musique" pour un mot raté
        /// </summary>
        void SignalerMotRate()
        {
            Console.Beep(4000, 200);
            Console.Beep(3500, 200);
            Console.Beep(3000, 200);
            Console.Beep(2500, 200);
            Console.Beep(2000, 200);
        }

        /// <summary>
        /// Joue une "musique" pour un mot trouvé
        /// </summary>
        void SignalerMotTrouve()
        {
            Console.Beep(3000, 200);
            Console.Beep(3500, 200);
            Console.Beep(4000, 200);
            Console.Beep(4500, 400);
            Console.Beep(3500, 200);
            Console.Beep(5000, 600);
        }


        #endregion


        #region Gestion d'une partie
        /// <summary>
        /// Affiche le mot masqué à la position requise
        /// </summary>
        /// <param name="mot">Le mot à afficher</param>
        /// <param name="posVerticale">La position verticale où le mot doit être affiché</param>
        /// <param name="posHorizontale">La position horizontale où le mot doit être affiché</param>
        void AfficherMotMasque(string mot, int posVerticale, int posHorizontale)
        {
            EffacerSectionJeu();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(posHorizontale - 2 / 2, posVerticale);
            for (int i = 0; i < 2 / 2; i++)
                Console.Write("\\");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(mot);
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < 2 / 2; i++)
                Console.Write("/");
            Console.SetCursorPosition(posHorizontale, posVerticale + 1);
            for (int i = 0; i < mot.Length; i++)
                Console.Write("+");
            Console.ResetColor();
        }

        /// <summary>
        /// Lit au clavier une lettre en A et Z
        /// </summary>
        /// <returns>Une lettre en A et Z.  Le résultat est toujours retourné en majuscules.</returns>
        char SaisirLettreEntreAetZ()
        {
            Console.SetCursorPosition(0, 0);
            char lettreEntreAetZ = char.ToUpper(Console.ReadKey().KeyChar);
            while (lettreEntreAetZ < 'A' || lettreEntreAetZ > 'Z')
            {
                lettreEntreAetZ = char.ToUpper(Console.ReadKey().KeyChar);
            }

            return lettreEntreAetZ;

        }
  
        /// <summary>
        /// Choisit un mot de manière aléatoire parmi une liste de mots possibles.
        /// </summary>
        /// <returns>Le mot choisi de manière aléatoire</returns>
        string ChoisirMotAleatoire()
        {
            string[] listeDeMotsPossibles = new string[] { "allo" };
            Random rnd = new Random();
            string motATrouver = listeDeMotsPossibles[rnd.Next(1, listeDeMotsPossibles.Length)].ToUpper();
            return motATrouver;
        }

        /// <summary>
        /// Joue une partie.  Le joueur doit trouver le maximum de mots.  La partie se
        /// termine lorsque le joueur a échoué un nombre déterminé de fois.
        /// </summary>
        void JouerUnePartie()
        {
            int nbMotsRatesDansPartie = 0;
            int nbMotsTrouvesDansPartie = 0;

            // Étape 1: Vider la console et afficher l'entête du jeu et le pied de page en mode partie.
            Console.Clear();
            AfficherEnteteJeu();
            AfficherPiedDePage();
            Console.CursorVisible = false;

            // Tant que la fin de partie n'est pas atteinte
            while (nbMotsRatesDansPartie < NB_MOTS_MAX_POUVANT_ETRE_RATES_DANS_PARTIE)
            {
                // Mettre à jour les statistiques de la partie en fonction de l'évolution de celle-ci.
                AfficherStatistiquesPartie(nbMotsRatesDansPartie, nbMotsTrouvesDansPartie);

                // Jouer un mot...
                bool motCourantTrouve = JouerUnMot(nbMotsTrouvesDansPartie, nbMotsRatesDansPartie);

                // ... et ajuster les statistiques.
                if (motCourantTrouve == false)
                {
                    nbMotsRatesDansPartie++;
                    // Rétroaction sonore d'un échec
                    SignalerMotRate();
                }
                else
                {
                    nbMotsTrouvesDansPartie++;
                    SignalerMotTrouve();
                }
            }
        }

        /// <summary>
        /// Joue un mot au complet. Le joueur tente de découvrir le mot caché et dispose d'un nombre maximal de
        /// tentatives pour y arriver.
        /// 
        /// Les paramètres reçus sont nécessaires pour faire l'affichage des statistiques de la partie à chaque tour.
        /// </summary>
        /// <param name="nbMotsTrouvesDansPartie">Le nombre de mots trouvés à date dans toute la partie</param>
        /// <param name="nbMotsRatesDansPartie">Le nombre de mots ratés à date dans toute la partie</param>
        /// <returns>true si le joueur a trouvé le mot, false sinon</returns>
        bool JouerUnMot(int nbMotsTrouvesDansPartie, int nbMotsRatesDansPartie)
        {
            int nbCaracteresManques = 0;

            // Choisir un mot à trouver parmi une liste de mots prédéterminés.
            string motATrouver = ChoisirMotAleatoire();

            // Encryper le mot en question
            string motMasque = MasquerMot(motATrouver);

            // Déterminer sa position de départ.  Position verticale par défaut, position horizontale au hasard. 
            Random rnd = new Random();
            int positionVerticaleMotCourant = POSITION_VERTICALE_DEBUT_JEU;
            int positionHorizontaleMotCourant = rnd.Next(1, Console.WindowWidth - motATrouver.Length - 2);

            int etatDuMotCourant = DeterminerEtatMotCourant(motATrouver, motMasque, nbCaracteresManques);
            // Tant que le mot courant (le mot masqué) n'est pas complètement découvert (i.-e. égal au mot à trouver)
            while (SUCCES_JOUEUR != etatDuMotCourant && ECHEC_JOUEUR != etatDuMotCourant)
            {
                // Afficher le mot masqué
                AfficherMotMasque(motMasque, positionVerticaleMotCourant, positionHorizontaleMotCourant);

                // Saisir la lettre courante
                char lettreCourante = SaisirLettreEntreAetZ();

                bool nouvelleLettreTrouvee = false;
                int indexLettreDansMotATrouver = motATrouver.IndexOf(lettreCourante);
                // Si le mot à trouver contient la lettre entrée...
                if (indexLettreDansMotATrouver != -1)
                {
                    // ... et que le mot découvert ne la contient pas (donc elle n'a pas encore été découverte
                    if (motMasque.IndexOf(lettreCourante) == -1)
                    {
                        nouvelleLettreTrouvee = true;

                        // Révéler la lettre dans le mot masqué. 
                        while (indexLettreDansMotATrouver != -1)
                        {
                            char[] temp = motMasque.ToCharArray();
                            temp[indexLettreDansMotATrouver] = lettreCourante;
                            motMasque = new string(temp);

                            indexLettreDansMotATrouver = motATrouver.IndexOf(lettreCourante, indexLettreDansMotATrouver);
                        }
                    }
                }

                // Faire descendre le mot si la lettre entrée est erronée.
                if (nouvelleLettreTrouvee == false)
                {
                    nbCaracteresManques++;
                    positionVerticaleMotCourant = POSITION_VERTICALE_DEBUT_JEU + nbCaracteresManques * VITESSE_VERTICALE;
                }

                // Déterminer l'état du mot courant
                etatDuMotCourant = DeterminerEtatMotCourant(motATrouver, motMasque, nbCaracteresManques);
            }
            return (ECHEC_JOUEUR != etatDuMotCourant);
        }

        /// <summary>
        /// Crée le mot "masqué".  Le mot masqué est équivalent au mot à trouver
        /// dans lequel toutes les lettres ont été remplacées par des astérisques.
        /// </summary>
        /// <param name="motAMasquer">Le mot à masquer (celui qui devra être trouvé par le joueur)</param>
        /// <returns>Le mot masqué</returns>
        public static string MasquerMot(string motAMasquer)
        {
            string motMasque = "";
            for (int i = 0; i < motAMasquer.Length - 1; i++)
                motMasque += "*";
            return motMasque;
        }

        /// <summary>
        /// Retourne le nom du programmeur du jeu
        /// </summary>
        /// <returns>Le nom du programmeur du jeu</returns>
        public static string ObtenirNomProgrammeur()
        {
            return NOM_PROGRAMMEUR;
        }


        /// <summary>
        /// Détermine l'état d'un mot à trouver.
        /// </summary>
        /// <param name="motATrouver">Le mot à trouver</param>
        /// <param name="motMasque">Le mot masqué dans lequel les lettres trouvées ont été révélées</param>
        /// <param name="nbCaracteresManques">Le nombre de lettres erronnées entrées.</param>
        /// <returns>ECHEC_JOUEUR si le nombre d'erreurs excède le maximum (pour un échec sur le mot)
        ///          JOUEUR_DOIT_CONTINUER  si ce n'est ni un échec, ni un succès (donc le joueur peut continuer à jouer ce mot
        ///          SUCCES_JOUEUR  si le joueur a trouvé toutes les lettres du mot (pour une "victoire" sur le mot)</returns>
        public static int DeterminerEtatMotCourant(string motATrouver, string motMasque, int nbCaracteresManques)
        {
            int resultatPartie = SUCCES_JOUEUR;

            if (nbCaracteresManques >= NB_MAX_ERREURS_DANS_MOT)
            {
                resultatPartie = ECHEC_JOUEUR;
            }
            else if (motATrouver != motMasque)
            {
                resultatPartie = JOUEUR_DOIT_CONTINUER;
            }
            return resultatPartie;

        }
    }


    #endregion

}
