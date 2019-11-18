using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Data.SQLite;

using SQLiteTools;


namespace Utils
{
    public static class CSVManager{
        public static void Parser(string fileName){
            SQLiteManager sqlManager;
            string csvFile = "customers.csv";
            
            if(File.Exists(fileName)){
                Console.Write("db.sqlite already exist, deleting it...");
                File.Delete("db.sqlite");
                Console.Write("[DONE]");
            }
            SQLiteConnection.CreateFile("db.sqlite");
            Console.WriteLine("");
            Console.Write("Searching for {0} ...",csvFile);
            string currentPath=System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = string.Format("{0}Resources\\{1}", Path.GetFullPath(Path.Combine(currentPath, @"..\..\")),csvFile);
            if (!File.Exists(filePath)){
                Console.WriteLine("[ERROR]");
                Console.WriteLine(filePath);
                Console.WriteLine("/!\\ File not found, make sure to put it in Resources folder.");
            }else{
                Console.WriteLine("[LOADED]");
                sqlManager = new SQLiteManager(fileName);
                StreamReader reader = File.OpenText(filePath);

                // first let's create tables
                sqlManager.CreateTable("customers","c_id int,forename text,surname text,dateOfBirth text");
                sqlManager.CreateTable("vehicules","v_id int , manufacturer text , model text , registrationNumber text , registrationDate text , engineSize int , owner_id int , vehicule_type text , interiorColor text , hasHelmetCase text");

                //then let's populate them

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
                        string registrationDate;

                        if(String.IsNullOrEmpty(split[4])) vehiculeId = -1;
                        else vehiculeId = Int32.Parse(split[4]);

;
                        string manufacturer=split[6];
                        string model=split[7];
                        string registrationNumber=split[5];
                        

                        if(String.IsNullOrEmpty(split[8] )) engineSize=0;
                        else engineSize=Int32.Parse(split[8]);

                        if(String.IsNullOrEmpty(split[9] )) registrationDate="0001-01-01";
                        else registrationDate=split[9];

                        string interiorColour = split[10];
                        string hasHelmetCase = split[11];
                        string VehiculeType=split[12];
                        

                        if(!sqlManager.CheckIfExist("c_id","customers",ownerId)){
                            string customerValues=ownerId+",'"+forename+"','"+lastname+"','"+birthDate+"'";
                            sqlManager.PopulateTable("customers","c_id,forename,surname ,dateOfBirth",customerValues);
                        }

                        if(!sqlManager.CheckIfExist("v_id","vehicules",vehiculeId)){
                            string vehiculeColumns="v_id, manufacturer , model , registrationNumber , registrationDate , engineSize , owner_id , vehicule_type , interiorColor , hasHelmetCase ";
                            string vehiculeValues=vehiculeId+",'"+manufacturer+"','"+model+"','"+registrationNumber+"','"+registrationDate+"','"+engineSize+"',"+ownerId+",'"+VehiculeType+"','"+interiorColour+"','"+hasHelmetCase+"'";
                            sqlManager.PopulateTable("vehicules",vehiculeColumns,vehiculeValues);
                        }
                        //sqlManager.SelectAllDebug();
                    }
                }
                //sqlManager.getCount("customers");
                long nCustomers = sqlManager.getCount("customers");
                long nVehicules = sqlManager.getCount("vehicules");

                sqlManager.SelectAllDebug();

                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("+ {0} customers where loaded",nCustomers);
                Console.WriteLine("+ {0} vehicules where loaded.",nVehicules);
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");

            }
        }
    }
}
