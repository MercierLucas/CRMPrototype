using System;
using System.Linq;
using Utils;
using CustomClasses;
using Vehicules;
using System.Collections.Generic;
using System.Globalization;

namespace Pinewood_CRM
{
    class Program
    {
        private static List<Customer> customers;
        private static List<Vehicule> vehicules;
        private static List<Car> cars;

        static void Main(string[] args)
        {
            
            Menu();

        }

        static void Menu(){
            Console.Clear();
            Console.WriteLine("  _____ _                                    _    _____ _____  __  __ ");
            Console.WriteLine(" |  __ (_)                                  | |  / ____|  __ \\|  \\/  |");
            Console.WriteLine(" | |__) | _ __   _____      _____   ___   __| | | |    | |__) | \\  / |");
            Console.WriteLine(" |  ___/ | '_ \\ / _ \\ \\ /\\ / / _ \\ / _ \\ / _` | | |    |  _  /| |\\/| |");
            Console.WriteLine(" | |   | | | | |  __/\\ V  V / (_) | (_) | (_| | | |____| | \\ \\| |  | |");
            Console.WriteLine(" |_|   |_|_| |_|\\___| \\_/\\_/ \\___/ \\___/ \\__,_|  \\_____|_|  \\_\\_|  |_|");
            Console.WriteLine("Welcome to the CRM prototype.This was made as part of my application to Pinewood");
            Console.WriteLine("1. Load a new CSV file");
            Console.WriteLine("2. Show customers");
            Console.WriteLine("3. - temp -");
            Console.WriteLine("4. - exit -");
            Console.Write("Select an option [1-4] >");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Enter a filename to parse (without extension) >");
                    string fileName=Console.ReadLine();
                    (customers,vehicules)=Utils.CSVManager.Parser(fileName);
                    displayMenu();
                    break;

                case "2":
                    displayMenu();
                    break;

                case "4":
                    break;

                default:
                    Menu();
                    break;
            }   
        }

        public static void displayMenu(){
            List <Customer> filteredCustomers=new List<Customer>();
            List <Vehicule> filteredVehicules=new List<Vehicule>();
            List <Car> filteredCars=new List<Car>();
            Console.WriteLine("What do you want to display?");
            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Filter customers by age");
            Console.WriteLine("3. Fitler by car register date");
            Console.WriteLine("4. Fitler by car engine");
            Console.WriteLine("5. Exit to main menu");
            Console.Write("Select an option [1-5] >");
            string choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    showTable(customers,vehicules,"Showing all customers");
                    displayMenu();
                    break;

                 case "2":
                    int minAge=0;
                    int maxAge=0;
                    bool tryMinAge=false;
                    bool tryMaxAge=false;

                    while(tryMinAge == false || tryMaxAge == false){
                        Console.Write("Min age to filter >");
                        string minAgeStr = Console.ReadLine();
                        Console.Write("Max age to filter >");
                        string maxAgeStr = Console.ReadLine();

                        tryMinAge = Int32.TryParse(minAgeStr,out minAge);
                        tryMaxAge = Int32.TryParse(maxAgeStr,out maxAge);

                        if(tryMaxAge == false || tryMinAge == false){
                            Console.WriteLine("Please enter only numbers");
                        }
                    }

                    filteredCustomers = Utils.FilterItems.filterByAge(customers,minAge,maxAge);
                    string message="Showing customers between "+minAge+" and "+maxAge+" :";
                    showTable(filteredCustomers,vehicules,message);
                    displayMenu();
                    break; 

                default:    
                    showTable(customers,vehicules,"Showing all customers");
                    displayMenu();
                    break;

                 case "3":
                    bool tryDate = false;
                    DateTime date=new DateTime();
                    while(!tryDate){
                        Console.Write("Enter a date (yyyy-MM-dd)>");
                        string dateStr = Console.ReadLine();
                        
                        tryDate = DateTime.TryParseExact(dateStr,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None, out date);

                        if(!tryDate)
                            Console.WriteLine("Invalid date type");
                    }
                    Console.WriteLine("You entered {0}",date.ToString("yyyy-MM-dd"));

                    (filteredCustomers,filteredVehicules) = Utils.FilterItems.filterByRegistrationDate(vehicules,customers,date);
                    showTable(filteredCustomers,filteredVehicules,"Showing customers who own a car registered before "+date.ToString("yyyy-MM-dd"));
                    displayMenu();
                    break;

                 case "4":
                    bool tryEngineSize = false;
                    int engineSize=0;
                    while(!tryEngineSize){
                        Console.Write("Enter an engine size value >");
                        string EngineSizeStr = Console.ReadLine();
                        
                        tryEngineSize = Int32.TryParse(EngineSizeStr, out engineSize);

                        if(!tryEngineSize)
                            Console.WriteLine("Please enter only numbers");
                    }
                    (filteredCustomers,filteredVehicules) = Utils.FilterItems.filterByEngineSize(vehicules,customers,engineSize);
                    showTable(filteredCustomers,filteredVehicules,"Showing customers who own a car with an engine size > "+engineSize);
                    displayMenu();
                    break;  

                case "5":
                    Menu();
                    break;
            }
        }

        public static void showTable(List<Customer> filteredCustomers,List<Vehicule> filterdVehicules,string message){
            Console.Clear();
            Console.WriteLine(message);
            string[] headers=new string[]{"CId","Forename","Surname","Birth","VId","RegNum","MAN","Model","EngineSize","RegDate","Colour","Helmet","VehicleType"};
            string txtHeader="";
            string txtBody="";
            foreach(string cat in headers){
                txtHeader+=String.Format("{0,15}",cat);
            }
            Console.WriteLine(txtHeader);

            foreach(Customer customer in filteredCustomers){
                txtBody+=String.Format("{0,15}",customer.getCustomerId());
                txtBody+=String.Format("{0,15}",customer.getForename());
                txtBody+=String.Format("{0,15}",customer.getLastname());
                txtBody+=String.Format("{0,15}",customer.getDateOfBirthAsString());

                List<Vehicule> ownerVehicules = Utils.FilterItems.getOwnerVehicules(filterdVehicules,customer.getCustomerId());

                foreach(Vehicule ownerVehicule in ownerVehicules){
                    txtBody+="\n";
                    txtBody+=String.Format("{0,75}",ownerVehicule.getVehiculeId());
                    txtBody+=String.Format("{0,15}",ownerVehicule.getRegristrationNumber());
                    txtBody+=String.Format("{0,15}",ownerVehicule.getManufacturer());
                    txtBody+=String.Format("{0,15}",ownerVehicule.getModel());
                    txtBody+=String.Format("{0,15}",ownerVehicule.getEngineSize());
                    txtBody+=String.Format("{0,15}",ownerVehicule.getRegristrationDateAsString());
                    if(ownerVehicule.getVehiculeType() == "Car"){
                        //Console.WriteLine(ownerVehicule.getInteriorColour());
                        txtBody+=String.Format("{0,15}",ownerVehicule.getInteriorColour());
                        txtBody+=String.Format("{0,15}","-");
                        //txtBody+=String.Format("{0,15} -");
                    }else if(ownerVehicule.getVehiculeType() == "Motorcycle"){
                        txtBody+=String.Format("{0,15}","-");
                        txtBody+=String.Format("{0,15}",ownerVehicule.getHasHelmetStorage());
                    }
                    txtBody+=String.Format("{0,15}",ownerVehicule.getVehiculeType());
                    
                }
                
                txtBody+="\n";
            }
            Console.WriteLine(txtBody);
            Console.WriteLine("\n \n *Cid (CustomerId) / VId (vehiculeId) / MAN (Manufacturer)/ RegNum (RegistrationNumber) / RegDate (RegistrationDate) / Colour (interior colour)");
        }
    }
}
