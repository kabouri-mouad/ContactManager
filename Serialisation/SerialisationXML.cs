using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml.Serialization;
using Data;

namespace Serialisation
{
    public class SerialisationXML
    {
        // Méthode pour faire la serialisation XML en utilisant XmlSerializer
        public void Serialization(string cheminVersFichier, Dossier racine)
        {
            // Instancier un objet de type XmlSerializer pour sérialiser les objets de type Dossier et Contact
            XmlSerializer xmls = new XmlSerializer(typeof(Dossier), new Type[] { typeof(Dossier), typeof(Contact) });
            // Ouvrir un fichier en mode écriture, avec la possibilité de créer le fichier s'il n'existe pas
            FileStream file = new FileStream(cheminVersFichier, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            // Instancier un objet de chiffrement DES pour sécuriser les données sérialisées
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            string key = "";
            CryptoStream cryptoStream = null;

            Console.WriteLine("Entrez votre clé de cryptage de longueur 8 (laissez vide pour utiliser votre SID): ");
            key = Console.ReadLine();

            // Si la clé est vide, utiliser le SID de l'utilisateur actuel comme clé de chiffrement
            if (key == "")                                             
            {
                key = WindowsIdentity.GetCurrent().User.ToString();
            }
            try
            {
                // Configuration de la clé et du vecteur d'initialisation (IV) pour le chiffrement DES
                desCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                desCryptoServiceProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");

                // Créer un flux de chiffrement pour écrire les données sérialisées dans le fichier de manière sécurisée
                cryptoStream = new CryptoStream(file, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                // Sérialiser l'objet racine (Dossier) dans le flux de chiffrement
                xmls.Serialize(cryptoStream, racine);
                Console.WriteLine("Opération réussie\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Erreur lors de la sérialisation du fichier\n");
            }

            // Fermer le flux de chiffrement et le fichier
            if (cryptoStream != null) cryptoStream.Close();
            if (file != null) file.Close();
        }

        // Méthode pour faire la déserialisation XML en utilisant XmlSerializer
        public Dossier Deserialization(string cheminVersFichier)
        {
            Dossier root = new Dossier();
            FileStream file = new FileStream(cheminVersFichier, FileMode.Open, FileAccess.Read, FileShare.None);
            // Instancier un objet XmlSerializer pour désérialiser les objets de type Dossier et Contact
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Dossier), new Type[] { typeof(Dossier), typeof(Contact) });
            // Instancier un objet de chiffrement DES pour décrypter les données lors de la désérialisation
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            string key = "";
            CryptoStream cryptoStream = null;

            Console.WriteLine("Entrez votre clé de cryptage de longueur 8 (laissez vide pour utiliser votre SID): ");
            key = Console.ReadLine();

            // Si la clé est vide, utiliser le SID de l'utilisateur actuel comme clé de chiffrement
            if (key == "")                                                
            {
                key = WindowsIdentity.GetCurrent().User.ToString();
            }

            try
            {
                // Configuration de la clé et du vecteur d'initialisation (IV) pour le chiffrement DES
                desCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                desCryptoServiceProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");

                // Créer un flux de chiffrement pour lire et décrypter les données sérialisées depuis le fichier
                cryptoStream = new CryptoStream(file, desCryptoServiceProvider.CreateDecryptor(desCryptoServiceProvider.Key, desCryptoServiceProvider.IV), CryptoStreamMode.Read);
                // Désérialiser les données depuis le flux de chiffrement et les assigner à l'objet root (Dossier)
                root = xmlSerializer.Deserialize(cryptoStream) as Dossier;
                Console.WriteLine("Fichier '{0}' chargé\n", cheminVersFichier);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Erreur lors de la désérialisation du fichier !!!!\n");
            }

            // Fermer le flux de chiffrement et le fichier
            if (cryptoStream != null) cryptoStream.Close();
            if (file != null) file.Close();

            return root;
        }

    }
}
