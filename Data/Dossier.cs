using Data;
using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class Dossier
    {
        private string nom;
        private DateTime dateCreation;
        private DateTime dateModification;
        private Dossier dossierContenu;   
        public  Dossier DossierParent;   
        private List<Contact> listeContacts;
        private int nombreContacts;

        public Dossier() { }

        public Dossier(string nom)
        {
            this.nom = nom;
            listeContacts = new List<Contact>();
            dateCreation = dateModification = DateTime.Now;
            nombreContacts = 0;
            dossierContenu = null;
        }

        public int NombreContacts
        {
            get => nombreContacts;
            set => nombreContacts = value;
        }

        public string Nom
        {
            get => nom;
            set
            {
                nom = value;
                dateModification = DateTime.Now;
            }
        }

        public List<Contact> getListeContacts
        {
            get => listeContacts;
        }

        public Dossier DossierContenu
        {
            get => dossierContenu;
            set => dossierContenu = value;
        }

        public DateTime DateCreation
        {
            get => dateCreation;
        }

        public DateTime DateModification
        {
            get => dateModification;
            set => dateModification = value;
        }

        public void AjouterDossier(string nom)
        {
            dossierContenu = new Dossier(nom); 
        }

        public void afficherDossier(int cmpt)
        {
            Console.WriteLine("[D] " + nom + " (création " + dateCreation + ")");
            foreach (Contact contact in listeContacts)
            {
                for (int i = 0; i < cmpt; i++) Console.Write(" ");
                contact.afficherContact();
            }
        }
    }
}
