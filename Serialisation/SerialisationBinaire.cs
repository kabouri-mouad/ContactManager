using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Runtime.Serialization;
using Data;

namespace Serialisation
{
    public class SerialisationBinaire
    {
        // Méthode pour faire la serialisation binaire en utilisant BinaryFormatter
        public void serialisation(string filePath, Dossier racine, ref string key)
        {
            // Instancier un objet de type BinaryFormatter pour sérialiser des objets
            BinaryFormatter bf = new BinaryFormatter();
            // Ouvrir un fichier en mode écriture, avec la possibilité de créer le fichier s'il n'existe pas
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            // Instancier un objet de chiffrement DES pour sécuriser les données sérialisées
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();                    
            CryptoStream cryptoStream = null;

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
                bf.Serialize(cryptoStream, racine);

                Console.WriteLine("Opération réussie !");
            }
            catch (FileNotFoundException f) { throw new FileNotFoundException(); }
            catch (SerializationException p) { throw new SerializationException(); }
            catch (Exception e) { Console.WriteLine("Réssayez et donnez une bonne cle"); }

            // Fermer le flux de chiffrement et le fichier
            if (cryptoStream != null) cryptoStream.Close();
            if (file != null) file.Close();
        }


        // Méthode pour faire la déserialisation en utilisant BinaryFormatter
        public Dossier deserialisation(string filePath, ref string key)
        {
            Dossier racine = new Dossier();
            // Ouvrir un fichier en mode lecture
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            BinaryFormatter bf = new BinaryFormatter();
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            CryptoStream cryptoStream = null;

            // Si key est vide, on utilise le SID
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

                // Désérialiser les données depuis le flux de chiffrement et les assigner à l'objet racine (Dossier)
                racine = bf.Deserialize(cryptoStream) as Dossier;

                Console.WriteLine("Opération réussie !");
            }
            catch (Exception e)
            {
                throw new Exception();
            }

            // Fermer le flux de chiffrement et le fichier
            if (cryptoStream != null) cryptoStream.Close();
            if (file != null) file.Close();

            return racine;
        }
    }
}
