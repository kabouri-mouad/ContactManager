using Serialisation;
using System;
using System.IO;
using System.Runtime.Serialization;
using Data;

namespace ContactManager
{
    public class Program
    {
        public static SerialisationBinaire serialisationBinaire = new SerialisationBinaire();
        public static SerialisationXML serialisationXml = new SerialisationXML();
        public static string key = "";
        public static string nomUtilisateur = Environment.UserName;

        // Méthode pour charger les fichies .db qui existent dans 'Documents'
        public static Dossier charger(ref string cheminVersFichier)
        {

            string[] fichiersTrouves = null;  // Fichiers de DB trouvés dans le dossier 'Documents'                                                                       
            string cheminVersMesDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Variable qui récupère l'emplacement du dossier 'Documents' de l'utilisateur courant
            fichiersTrouves = Directory.GetFiles(cheminVersMesDocuments, "*.db"); // Cherche les fichiers .db dans 'Documents'
            string monFichier = "";
            Dossier result = new Dossier("result");

            if (fichiersTrouves.Length == 0) // Si aucun fichier est trouvé on créé un, sinon on charge le premier qu'on a trouvé
            {
                Console.WriteLine("Aucun fichier de DB trouvé !");
                cheminVersFichier = creationFichier();
            }
            else
            {
                Console.WriteLine("Voici vos fichiers qui sont déjà enregistrés :\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (string e in fichiersTrouves)
                {
                    Console.WriteLine(e);
                }
                Console.ResetColor();
                Console.WriteLine("\nDonner le nom du fichier que vous voulez charger : (sans .db)");
                Console.Write(nomUtilisateur + ">");
                monFichier = Console.ReadLine();
                cheminVersFichier = Path.Combine(cheminVersMesDocuments, monFichier + ".db");

                try
                {
                    Console.WriteLine("Entrez votre clé de cryptage (vide => utiliser votre SID): ");
                    Console.Write(nomUtilisateur + ">");
                    key = Console.ReadLine();
                    Gestion.racine = serialisationBinaire.deserialisation(cheminVersFichier, ref key);                           //la racine prend le retour de la deserialisation
                    Console.WriteLine("\nChargement du fichier '{0}' ...", cheminVersFichier);
                    Gestion.dossierCourant = dernierDossier(Gestion.racine);                                                        // On récupère le dernier dossier créé, comme ça le dossier_courant pointe vers lui

                    Console.Write("Le fichier ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(cheminVersFichier);
                    Console.ResetColor();
                    Console.WriteLine(" où vos données sont enregistrées a été trouvé et il sera chargé.");
                    Console.WriteLine("N'oubliez pas d'enregistrer vos modifications !\n");
                }
                catch (Exception e) { throw new Exception(); }

            }

            return Gestion.dossierCourant;
        }


        // Methode pour crée un nouveau fichier
        public static string creationFichier()
        {
            string nom_fichier = "";
            string chemin_mes_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);            //recuperer le chemin vers 'mes documents'
            string chemin_fichier = "";

            Console.WriteLine("Merci de saisir le nom du fichier à crée (vide pour id windows): ");

            nom_fichier = Console.ReadLine();

            if (nom_fichier == "")                                                                               // Si vide alors on utilise l'id windows comme nom
            {
                nom_fichier = Environment.UserName;
            }

            chemin_fichier = Path.Combine(chemin_mes_documents, nom_fichier + ".db");                      // Combiner le chemin du dossier 'Documents' avec le nom du fichier et son extension
            Gestion.dossierCourant = Gestion.racine;                                            // envoyer dle dossier courant depuis le debut  
            Gestion.racine.DossierContenu = null;

            Console.Write("Le fichier ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(chemin_fichier);
            Console.ResetColor();
            Console.WriteLine(" a été créé avec succès !");
            Console.WriteLine("N'oubliez pas d'enregistrer vos modifications !");


            return chemin_fichier;                                                              // On retourne filePath pour la stocker dans la variable correspondante dans Main
        }

        // Méthode pour enregister les fichies en utilisant la serialisation binaire
        public static void enregistrer(string cheminVersFichier)
        {
            Console.WriteLine("Pour enregistrer, entrez votre clé de cryptage de longueur 8 (laissez vide pour utiliser votre SID): ");
            key = Console.ReadLine();

            Console.WriteLine("Enregistrement du fichier '{0}' ...", cheminVersFichier);
            serialisationBinaire.serialisation(cheminVersFichier, Gestion.racine, ref key);
        }

        // Chercher le dernier dossier créé dans une arborescence 
        public static Dossier dernierDossier(Dossier root)
        {
            Dossier dossierCourant = root;
            while (dossierCourant.DossierContenu != null)
            {
                dossierCourant = dossierCourant.DossierContenu;
            }
            return dossierCourant;
        }

        public static void Main(string[] args)
        {
            Gestion gestion = new Gestion();
            string cheminFichier = "vide";
            bool ok = false;
            int i = 0;
            string choix = "";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("==================== Vous voulez charger ou créer un fichier '.db' ? ==================");
            Console.WriteLine("= [Charger] : taper 1                                                                 =");
            Console.WriteLine("= [Créer]   : taper 2                                                                 =");
            Console.WriteLine("=======================================================================================");
            Console.ResetColor();
            Console.Write(nomUtilisateur + "@>");
            choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    while (i < 3)
                    {
                        try
                        {
                            charger(ref cheminFichier);
                            i = 3;
                            ok = true;
                        }
                        catch (FileNotFoundException p)
                        {
                            Console.Write("\nCa vous reste ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write((2 - i) + " tentative(s) !");
                            Console.ResetColor();
                            Console.WriteLine("Réssayer !\n");
                            i++;
                        }
                        catch (SerializationException m)
                        {
                            Console.WriteLine("La clé utilisée est incorrecte !");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.Write("\nCa vous reste ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine((2 - i) + " tentative(s) !");
                            Console.ResetColor();
                            i++;
                            if(i == 3)
                            {
                                try
                                {
                                    File.Delete(cheminFichier);
                                    Console.WriteLine("Le fichier a été supprimé avec succès.");
                                }
                                catch (IOException ioe)
                                {
                                    Console.WriteLine("Une erreur est survenue lors de la suppression du fichier : " + ioe.Message);
                                }
                            }
                            key = "";
                            cheminFichier = "";

                        }

                    }
                    break;

                case "2":
                    cheminFichier = creationFichier();
                    ok = true;
                    break;

                default:
                    break;

            }


            if (ok)
            {
                string[] input;
                bool stop = false;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nBonjour ! Veuillez taper l'action que vous voulez");
                Console.WriteLine("========================== Les actions possibles ========================");
                Console.WriteLine("= [1] - taper sortir                                                    =");
                Console.WriteLine("= [2] - taper afficher                                                  =");
                Console.WriteLine("= [3] - taper charger                                                   =");
                Console.WriteLine("= [4] - taper enregistrer                                               =");
                Console.WriteLine("= [5] - taper ajouterdossier [nom du dossier]                           =");
                Console.WriteLine("= [6] - taper ajoutercontact [prenom] [nom] [societe] [courriel] [lien] =");
                Console.WriteLine("=========================================================================");
                Console.ResetColor();

                while (!stop)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(nomUtilisateur + ">");
                    input = Console.ReadLine().Split(' ');
                    Console.ResetColor();

                    try
                    {
                        switch (input[0])
                        {
                            case "sortir":
                                stop = true;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("Au revoir " + nomUtilisateur + " :)");
                                Console.ResetColor();
                                break;

                            case "afficher":
                                gestion.afficherGestion();
                                break;

                            case "charger":
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Enregistrer les modifications ? 1/oui, 2/non. Tapez 1 ou 2\n (NB : Si vous choisissez oui vous devez saisir  une nouvelle cle)");
                                choix = Console.ReadLine();
                                Console.ResetColor();

                                if (int.Parse(choix) == 1)
                                {
                                    enregistrer(cheminFichier);
                                    Gestion.dossierCourant = charger(ref cheminFichier);
                                }
                                else if (int.Parse(choix) == 2) Gestion.dossierCourant = charger(ref cheminFichier);
                                break;

                            case "enregistrer":
                                enregistrer(cheminFichier);
                                break;

                            case "ajouterdossier":
                                gestion.ajouterDossier(input[1]);
                                break;

                            case "ajoutercontact":
                                gestion.ajouterContact(input[2], input[1], input[4], input[3], input[5]);
                                break;

                            default:
                                Console.WriteLine("Instruction Iconnue :)");
                                break;
                        }
                    }
                    catch (FileNotFoundException y)
                    {
                        Console.WriteLine("Désolé, ce fichier n'existe pas !");
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("Paramètres de saisis insuffisants !");
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("la clé utilisé est incorrecte, réssayez plus tard !");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Une erreur est survenue !");
                    }
                }
            }
        }
    }
}
