using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Data
{
    [Serializable]
    public class Contact
    {
        private string nom;
        private string prenom;
        private string courriel;
        private string societe;
        private string lien;
        private DateTime dateCreation;
        private DateTime dateModification;
        private Dossier dossierParent;
        private int position;          

        public Contact()
        { }

        public Contact(string nom, string prenom, string courriel, string societe, string lien)
        {

            try
            {
                MailAddress mailAddressValid = new MailAddress(courriel);
                this.courriel = courriel;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Adresse e-mail non valide ! Veuillez réessayer ultérieurement. (Le champ e-mail reste vide pour le moment)");
            }
            finally
            {
                this.nom = nom;
                this.prenom = prenom;
                this.societe = societe;
                this.lien = lien;
                dateCreation = DateTime.Now;
                dateModification = DateTime.Now;
                dossierParent = Gestion.dossierCourant;
            }
        }

        public int Position
        {
            get => position;
            set => position = value;
        }

        public Dossier DossierParent
        {
            get => dossierParent;
            set => dossierParent = value;
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
        public string Prenom
        {
            get => prenom;
            set
            {
                prenom = value;
                dateModification = DateTime.Now;
            }
        }
        public string Courriel
        {
            get => courriel;
            set
            {
                courriel = value;
                dateModification = DateTime.Now;
            }
        }
        public string Societe
        {
            get => societe;
            set
            {
                societe = value;
                dateModification = DateTime.Now;
            }
        }
        public string Lien
        {
            get => lien;
            set
            {
                lien = value;
                dateModification = DateTime.Now;
            }
        }
        public DateTime DateCreation
        {
            get => dateCreation;
        }
        public DateTime DateModification
        {
            get => dateModification;
        }

        public void afficherContact()
        {
            Console.WriteLine("| [C] " + nom + ", " + prenom + ", (" + societe + "), Email:" + courriel + ", Link:" + lien);
        }

        // Destructuer
        ~Contact()
        {

        }
    }
}
