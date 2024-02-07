using Data;
using System;

namespace Data
{
    [Serializable] // Indiquer que les instances de la classe peuvent être sérialisées
    public class Gestion
    {
        public static Dossier racine;
        public static Dossier dossierCourant;

        public Gestion()
        {
            racine = new Dossier("Root"); // Le root de l'arborescence
            dossierCourant = racine.DossierParent = racine;
        }

        public void ajouterContact(string nom, string prenom, string courriel, string societe, string lien)
        {
            Contact nouveauContact = new Contact(nom, prenom, courriel, societe, lien);
            dossierCourant.getListeContacts.Add(nouveauContact);
            nouveauContact.Position = dossierCourant.NombreContacts++;
            Console.WriteLine("Contact " + prenom + " ajouté sous dossier en position 1");
            //Mise à jour de la date de modification du dossier parent
            nouveauContact.DossierParent.DateModification = DateTime.Now;
        }

        public void ajouterDossier(string nom)
        {
            Dossier nouveauDossier = new Dossier(nom);
            nouveauDossier.DossierParent = dossierCourant;
            dossierCourant.DossierContenu = nouveauDossier;
            dossierCourant = nouveauDossier;
            nouveauDossier.DossierParent.DateModification = DateTime.Now;
            Console.WriteLine("Dossier '" + nom + "' ajouté sous dossier en position 1");
        }

        public Contact chercherContact(string nom, string prenom)
        {
            Dossier dossierCourant = racine;
            Contact contact = null;
            bool existe = false;

            //Un seul dossier qui est la racine 
            if (Gestion.dossierCourant == racine)
            {
                contact = dossierCourant.getListeContacts.Find(c => c.Nom == nom && c.Prenom == prenom);
            }
            else
            {
                while (dossierCourant != Gestion.dossierCourant && !existe)
                {
                    contact = dossierCourant.getListeContacts.Find(c => c.Nom == nom && c.Prenom == prenom);
                    existe = contact != null;

                    if (!existe)
                    {
                        dossierCourant = dossierCourant.DossierContenu;
                    }
                }
            }

            return contact;
        }

        public void supprimerContact(string nom, string prenom)
        {
            Contact contact = chercherContact(nom, prenom);

            if (contact != null)
            {
                contact.DossierParent.getListeContacts.RemoveAt(contact.Position);
                Console.WriteLine("le contact a été supprimé avec succès !");

                //Mise à jour de la date de modification du dossier parent
                contact.DossierParent.DateModification = DateTime.Now;
            }
        }

        public Dossier chercherPrecedentDossier(string nom)
        {
            Dossier dossierPrecedent = racine;

            while (dossierPrecedent.DossierParent != null && dossierPrecedent.DossierParent.Nom != nom)
            {
                dossierPrecedent = dossierPrecedent.DossierParent;
            }

            return dossierPrecedent;
        }

        public void supprimerDossier(string nom)
        {
            Dossier dossier = chercherPrecedentDossier(nom);
            dossier.DossierContenu = null;
            Gestion.dossierCourant = dossier;
            dossier.DateModification = DateTime.Now;
        }


        public void afficherGestion()
        {
            Dossier dossierCourant = racine;
            int compteur = 0;

            while (dossierCourant != null)
            {
                for (int i = 0; i < compteur; i++) Console.Write(" ");
                dossierCourant.afficherDossier(compteur);
                dossierCourant = dossierCourant.DossierContenu;
                compteur++;
            }
        }
    }
}
