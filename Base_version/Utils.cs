using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomClasses;
using Vehicules;
using System.Linq;

namespace Utils
{
    public static class CSVManager{
        public static (List<Customer>customers,List<Vehicule>vehicules) Parser(string fileName){
            Console.Write("Searching for {0}.csv ...",fileName);
            string currentPath=System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = string.Format("{0}Resources\\{1}.csv", Path.GetFullPath(Path.Combine(currentPath, @"..\..\")),fileName);
            if (!File.Exists(filePath)){
                Console.WriteLine("[ERROR]");
                Console.WriteLine(filePath);
                Console.WriteLine("/!\\ File not found, make sure to put it in Resources folder.");
                return (null,null);
            }
                
            
            else{
                Console.WriteLine("[LOADED]");

                List<string[]> data = new List<string[]>();

                List<Customer> customers = new List<Customer>();
                List<Car> cars = new List<Car>();
                List<Motorcycle> motorcycles = new List<Motorcycle>();

                List<Vehicule> vehicules = new List<Vehicule>();

                StreamReader reader = File.OpenText(filePath);

                string currentLine;

                while((currentLine = reader.ReadLine()) != null){
                    string[] items = currentLine.Split('\t');
                    string[] split = items[0].Split(',');

                    if(split[0] != "CustomerId"){
                        // if the current line isn't the header
                        int ownerId = Int32.Parse(split[0]);
                        string forename = split[1];
                        string lastname = split[2];
                        string birthDate = split[3];

                        int vehiculeId;
                        int engineSize;

                        if(String.IsNullOrEmpty(split[4])) vehiculeId = -1;
                        else vehiculeId = Int32.Parse(split[4]);

                        string manufacturer=split[6];
                        string model=split[7];
                        string regristrationNumber=split[5];
                        string regristrationDate=split[9];

                        if(String.IsNullOrEmpty(split[8] )) engineSize=0;
                        else engineSize=Int32.Parse(split[8]);

                        string VehiculeType=split[12];

                        Customer tempCustomer = new Customer(forename,lastname,birthDate,ownerId);
                        
                        //check if the customer isn't already registered
                        bool customerAlreadyRegisterd = customers.Any(x=> x.getCustomerId()==ownerId);
                        if(!customerAlreadyRegisterd) customers.Add(tempCustomer);

                        if(VehiculeType== "Car"){
                            string interiorColour = split[10];
                            Car tempCar = new Car(vehiculeId,manufacturer,model,regristrationNumber,regristrationDate,engineSize,ownerId,VehiculeType,interiorColour);
                            
                            cars.Add(tempCar);
                            vehicules.Add(tempCar);

                        }else if(VehiculeType == "Motorcycle"){
                            bool hasHelmetCase;
                            if(split[11] == "No" ) hasHelmetCase=false;
                            else hasHelmetCase = true;

                            Motorcycle tempMoto = new Motorcycle(vehiculeId,manufacturer,model,regristrationNumber,regristrationDate,engineSize,ownerId,VehiculeType,hasHelmetCase);
                            vehicules.Add(tempMoto);
                            motorcycles.Add(tempMoto);
                        }
                    
                    }
                    
                    data.Add(split);
                }
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("+ {0} customers where loaded",customers.Count);
                Console.WriteLine("+ {0} cars where loaded.",cars.Count);
                Console.WriteLine("+ {0} motocycles where loaded.",motorcycles.Count);
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("1st customer: {0}",customers[0].getForename());


                return (customers,vehicules);
                //Console.WriteLine("1st element: {0}",data[0][0].GetType());
            }   
            
        }
    }

    public static class FilterItems{
        public static List<Vehicule> getOwnerVehicules(List<Vehicule> vehicules,int cId){

            IEnumerable<Vehicule> tempVehicule = from vehicule in vehicules 
                    where vehicule.getOwnerId() == cId
                    select vehicule;

            List<Vehicule> ownerVehicules = tempVehicule.ToList();

            return ownerVehicules;
        }
        public static List<Customer> filterByAge(List<Customer> customers,int minAge,int maxAge){

            IEnumerable<Customer> tempCustomer = from customer in customers 
                    where customer.getAge() >= minAge && customer.getAge() <= maxAge
                    select customer;
                    
            return tempCustomer.ToList();
        }

        public static (List<Customer> filteredCustomers,List<Vehicule> fitleredVehicules) filterByEngineSize(List<Vehicule> vehicules,List<Customer> customers,int engineSize){

            List<Customer> filteredCustomers= new List<Customer>();

            IEnumerable<Vehicule> fitleredVehicules = from vehicule in vehicules 
                    where vehicule.getEngineSize() >= engineSize
                    select vehicule;

            foreach(Vehicule vehicule in fitleredVehicules.ToList()){
                IEnumerable<Customer> tempCustomers = from customer in customers
                    where customer.getCustomerId() == vehicule.getOwnerId()
                    select customer;
                
                bool customerAlreadyRegisterd = filteredCustomers.Any(x=> x.getCustomerId()==tempCustomers.ToList()[0].getCustomerId());
                if(!customerAlreadyRegisterd) filteredCustomers.Add(tempCustomers.ToList()[0]);
                // only 1 owner per vehicule
                
            }
                       
            Console.WriteLine("For engine >={0} , {1} cars found",engineSize,fitleredVehicules.ToList().Count);
            Console.WriteLine("Found {0} customers ",filteredCustomers.Count);

            return (filteredCustomers,fitleredVehicules.ToList()); 
        }

        public static (List<Customer> filteredCustomers,List<Vehicule> fitleredVehicules) filterByRegistrationDate(List<Vehicule> vehicules,List<Customer> customers,DateTime date){
            List<Customer> filteredCustomers= new List<Customer>();

            IEnumerable<Vehicule> fitleredVehicules = from vehicule in vehicules 
                    where vehicule.getRegristrationDate() < date
                    select vehicule;

            foreach(Vehicule vehicule in fitleredVehicules.ToList()){
                IEnumerable<Customer> tempCustomers = from customer in customers
                    where customer.getCustomerId() == vehicule.getOwnerId()
                    select customer;

                bool customerAlreadyRegisterd = filteredCustomers.Any(x=> x.getCustomerId()==tempCustomers.ToList()[0].getCustomerId());
                if(!customerAlreadyRegisterd) filteredCustomers.Add(tempCustomers.ToList()[0]);
                // only 1 owner per vehicule
            }
            //Console.WriteLine("For date before {0} , {1} cars found",date.ToString("yyyy-MM-dd"),filteredCars.ToList().Count);

            return (filteredCustomers,fitleredVehicules.ToList()); 
        }
    }
}
