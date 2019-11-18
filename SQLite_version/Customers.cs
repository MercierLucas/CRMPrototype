using System;
using System.IO;
using System.Collections.Generic;
using Vehicules;

namespace CustomClasses
{
    public class Customer{

        private string forename;

        private string lastname;

        private DateTime dateOfBirth;

        private int customerId;

        private List<Vehicule> ownedVehicules;

        public Customer(int customerId,string forename,string lastname,string dateOfBirth){
            this.forename=forename;

            this.lastname=lastname;
            Console.WriteLine(dateOfBirth);
            this.dateOfBirth=DateTime.Parse(dateOfBirth);
            Console.WriteLine(this.getDateOfBirthAsString());

            this.customerId = customerId;

            this.ownedVehicules = new List<Vehicule>();
        }

        public void addVehicules(Vehicule vehicule){
            this.ownedVehicules.Add(vehicule);
        }

        public List<Vehicule> getOwnedVehicules(){
            return this.ownedVehicules;
        }

        public string getForename()
        {
            return this.forename;
        }

        public void setForename(string forename)
        {
            this.forename = forename;
        }

        public string getLastname()
        {
            return this.lastname;
        }

        public void setLastname(string lastname)
        {
            this.lastname = lastname;
        }

        public DateTime getDateOfBirth()
        {
            return this.dateOfBirth;
        }

        public string getDateOfBirthAsString()
        {
            return this.dateOfBirth.ToString("yyyy-MM-dd");
        }

        public void setDateOfBirth(DateTime dateOfBirth)
        {
            this.dateOfBirth = dateOfBirth;
        }

        public int getCustomerId()
        {
            return this.customerId;
        }

        public void setCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        public int getAge(){
            return (DateTime.Today.Year-this.dateOfBirth.Year);
        }
    }
}
