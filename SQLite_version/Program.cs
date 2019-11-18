using System;
using System.Data.SQLite;
using Utils;
using SQLiteTools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomClasses;
using Vehicules;

using System.Globalization;

namespace prototype_SQL_C_SHARP
{
    class Program
    {
        private static SQLiteConnection connection;
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
            Console.WriteLine("4. - exit -");
            Console.Write("Select an option [1-4] >");
            string choice = Console.ReadLine();
            switch (choice)
            {
                default:
                    Menu();
                    break;

                case "1":
                    Utils.CSVManager.Parser("db.sqlite");
                    displayMenu();
                    break;

                case "4":
                    break;
            }
        }

        static void waitForInput(){
            Console.Write("Press enter to continue");
            string choice = Console.ReadLine();
        }

        static void displayMenu(){
            SQLiteManager sqlManager = new SQLiteManager("db.sqlite");
            List<Customer> customers;
            
            Console.WriteLine("What do you want to display?");
            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Filter customers by age");
            Console.WriteLine("3. Fitler by car register date");
            Console.WriteLine("4. Fitler by car engine");
            Console.WriteLine("5. Exit to main menu");
            Console.Write("Select an option [1-5] >");
            string choice = Console.ReadLine();
            switch(choice){
                default:
                    displayMenu();
                    break;

                case "1":
                    customers =  sqlManager.SelectAll();
                    showTables(customers,"Showing all customers and their vehicles");
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
                    customers = sqlManager.SelectByAge(minAge,maxAge);
                    showTables(customers,"Showing all customers bewteen "+minAge+" and "+maxAge);
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
                    customers = sqlManager.SelectByRegistrationDate(date);
                    showTables(customers,"Showing all customers and their vehicles registered before "+date.ToString("yyyy-MM-dd"));
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
                    customers = sqlManager.SelectByEngineSize(engineSize);
                    showTables(customers,"Showing all customers and their vehicles with engineSize > "+ engineSize);
                    displayMenu();
                    break;

                case "5":
                    Menu();
                    break;
            }
        }

        static void showTables(List<Customer> customers,string message){
            Console.Clear();

            Console.WriteLine(message);
            Console.WriteLine("");
            string[] headers=new string[]{"CId","Forename","Surname","Birth","VId","RegNum","MAN","Model","EngineSize","RegDate","Colour","Helmet","VehicleType"};
            string txtHeader="";
            string txtBody="";
            foreach(string cat in headers){
                txtHeader+=String.Format("{0,15}",cat);
            }
            Console.WriteLine(txtHeader);

            foreach(Customer customer in customers){
                txtBody+=String.Format("{0,15}",customer.getCustomerId());
                txtBody+=String.Format("{0,15}",customer.getForename());
                txtBody+=String.Format("{0,15}",customer.getLastname());
                txtBody+=String.Format("{0,15}",customer.getDateOfBirthAsString());
                txtBody+="\n";

                foreach(Vehicule vehicule in customer.getOwnedVehicules()){
                    txtBody+="\n";
                    txtBody+=String.Format("{0,75}",vehicule.getVehiculeId());
                    txtBody+=String.Format("{0,15}",vehicule.getRegristrationNumber());
                    txtBody+=String.Format("{0,15}",vehicule.getManufacturer());
                    txtBody+=String.Format("{0,15}",vehicule.getModel());
                    txtBody+=String.Format("{0,15}",vehicule.getEngineSize());
                    txtBody+=String.Format("{0,15}",vehicule.getRegristrationDateAsString());
                    if(vehicule.getVehiculeType() == "Car"){
                        //Console.WriteLine(ownerVehicule.getInteriorColour());
                        txtBody+=String.Format("{0,15}",vehicule.getInteriorColour());
                        txtBody+=String.Format("{0,15}","-");
                        //txtBody+=String.Format("{0,15} -");
                    }else if(vehicule.getVehiculeType() == "Motorcycle"){
                        txtBody+=String.Format("{0,15}","-");
                        txtBody+=String.Format("{0,15}",vehicule.getHasHelmetCase());
                    }
                    txtBody+=String.Format("{0,15}",vehicule.getVehiculeType());
                    txtBody+="\n";
                }
            }

            Console.WriteLine(txtBody);
        }
    }
}
