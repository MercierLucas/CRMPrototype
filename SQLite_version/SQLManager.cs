using System;
using System.Data.SQLite;
using Utils;
using System.Collections;
using System.Collections.Generic;

using CustomClasses;
using Vehicules;

using System.Linq;


namespace SQLiteTools
{
    class SQLiteManager
    {
        /// this class gather all redundant tools and methods used in SQLite for C# project
        private SQLiteConnection connection;
        private SQLiteCommand command;

        public SQLiteManager(string fileName)
        {

            this.connection= new SQLiteConnection("Data Source="+fileName+";Version=3;");

            this.command = new SQLiteCommand();
        }

        public void CreateTable(string tableName,string columns){
            this.connection.Open();
            string sql = "create table "+ tableName+" ( "+columns+" )";

            this.command = new SQLiteCommand(sql, this.connection);
            command.ExecuteNonQuery();
            this.Close();
        }
 
        public void PopulateTable(string tableName,string columns,string values){
            this.connection.Open();
            string sql = "insert into "+ tableName+" ( "+columns+" ) values ( "+values+" )";
            //Console.WriteLine(sql);
            this.command = new SQLiteCommand(sql, this.connection);
            command.ExecuteNonQuery();
            this.Close();
        }

        public void Close(){
            this.connection.Close();
        }

        public void SelectAllDebug(){
            this.connection.Open();
            string sql = "select * from vehicules";
            this.command = new SQLiteCommand(sql, this.connection);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("v_id: " + reader["v_id"] + "\tRegnum: " + reader["registrationNumber"]);

            this.Close();
        }

        public bool CheckIfExist(string field,string tableName,int id){
            this.connection.Open();
            string sql = "select count(*) as c from "+tableName+" where "+field+"="+id;
            this.command = new SQLiteCommand(sql, this.connection);
            long count = (long)command.ExecuteScalar();
            this.Close();
            if(count!=0) return true;
            return false;
        }

        public long getCount(string tableName){
            this.connection.Open();
            string sql = "select count(*) as c from "+tableName;
            this.command = new SQLiteCommand(sql, this.connection);
            long count = (long)command.ExecuteScalar();
            this.Close();
            return count;
        }

        public List<Customer> fetchReaderResults(string request){
            this.connection.Open();

            this.command = new SQLiteCommand(request, this.connection);
            List<Customer> customers = new List<Customer>();
            List<Vehicule> vehicules = new List<Vehicule>();

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()){
                Customer temp = new Customer((int)reader["c_id"],(string)reader["forename"],(string)reader["surname"],(string)reader["dateOfBirth"]);


                int vId=(int)reader["v_id"];
                string manufacturer=(string)reader["manufacturer"];
                string model=(string)reader["model"];
                string registrationNumber=(string)reader["registrationNumber"];
                string registrationDate=(string)reader["registrationDate"];
                int engineSize=(int)reader["engineSize"];
                int ownerId=(int)reader["owner_id"];
                string vehiculeType=(string)reader["vehicule_type"];
                string interiorColor=(string)reader["interiorColor"];
                string hasHelmetCase=(string)reader["hasHelmetCase"];

                Vehicule tempVehicule = new Vehicule(vId,manufacturer,model,registrationNumber,registrationDate,engineSize,ownerId,vehiculeType,interiorColor,hasHelmetCase);
                 
                Console.WriteLine(temp.getCustomerId());
                bool customerAlreadyAdded = customers.Any(x=> x.getCustomerId()==temp.getCustomerId());
                if(!customerAlreadyAdded){
                    // if not already added to list
                    temp.addVehicules(tempVehicule);
                    customers.Add(temp);
                }else{
                    // if the customer has already been added we add the vehicule to him
                    Console.WriteLine("doublon id:{0}",temp.getCustomerId());
                    customers.Find(x => x.getCustomerId()==temp.getCustomerId()).addVehicules(tempVehicule);
                } 
            }
                
            this.Close();

            return customers;
        }

        public List<Customer> SelectAll(){
            string sql = "select * from customers join vehicules on customers.c_id=vehicules.owner_id";
            return fetchReaderResults(sql);
        }

        public List<Customer> SelectByAge(int minAge,int maxAge){
            string sql = "select * from customers join vehicules on customers.c_id=vehicules.owner_id where (julianday()-julianday(customers.dateOfBirth))/365 > "+minAge+" and (julianday()-julianday(customers.dateOfBirth))/365 < "+maxAge;
            return fetchReaderResults(sql);
        }

        public List<Customer> SelectByRegistrationDate(DateTime date){
            string sql ="select * from customers join vehicules on customers.c_id=vehicules.owner_id where (julianday(vehicules.registrationDate)-julianday('"+date.ToString("yyyy-MM-dd")+"')) < 0 ;";
            return fetchReaderResults(sql);
        }

        public List<Customer> SelectByEngineSize(int engineSize){
            string sql="select * from customers join vehicules on customers.c_id=vehicules.owner_id where vehicules.engineSize > "+engineSize;
            return fetchReaderResults(sql);
        }

    
        
    }
}
