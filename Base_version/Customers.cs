using System;
using System.IO;
using System.Collections.Generic;

namespace CustomClasses
{
    public class Customer{

        private string forename;

        private string lastname;

        private DateTime dateOfBirth;

        private int customerId;

        public Customer(string forename,string lastname,string dateOfBirth,int customerId){
            this.forename=forename;

            this.lastname=lastname;

            this.dateOfBirth=DateTime.Parse(dateOfBirth);

            this.customerId = customerId;
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
