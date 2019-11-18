using System;
using System.IO;
using System.Collections.Generic;

namespace Vehicules
{
    public class Vehicule{

        private int vehiculeId;
        private string manufacturer;

        private string model;

        private string regristrationNumber;

        private DateTime regristrationDate;

        private int engineSize;

        private int ownerId;

        private string vehiculeType;

        private string interiorColour;

        private string hasHelmetCase;

        public string getInteriorColour()
        {
            return this.interiorColour;
        }

        public void setInteriorColour(string interiorColour)
        {
            this.interiorColour = interiorColour;
        }

        public string getHasHelmetCase()
        {
            return this.hasHelmetCase;
        }

        public void setHasHelmetCase(string hasHelmetCase)
        {
            this.hasHelmetCase = hasHelmetCase;
        }

        public string getVehiculeType()
        {
            return this.vehiculeType;
        }

        public void setVehiculeType(string vehiculeType)
        {
            this.vehiculeType = vehiculeType;
        }


        public int getVehiculeId()
        {
            return this.vehiculeId;
        }

        public void setVehiculeId(int vehiculeId)
        {
            this.vehiculeId = vehiculeId;
        }

        public string getManufacturer()
        {
            return this.manufacturer;
        }

        public void setManufacturer(string manufacturer)
        {
            this.manufacturer = manufacturer;
        }

        public string getModel()
        {
            return this.model;
        }

        public void setModel(string model)
        {
            this.model = model;
        }

        public string getRegristrationNumber()
        {
            return this.regristrationNumber;
        }

        public void setRegristrationNumber(string regristrationNumber)
        {
            this.regristrationNumber = regristrationNumber;
        }

        public DateTime getRegristrationDate()
        {
            return this.regristrationDate;
        }

        public string getRegristrationDateAsString()
        {
            return this.regristrationDate.ToString("yyyy-MM-dd");
        }

        public void setRegristrationDate(string regristrationDate)
        {
            this.regristrationDate = DateTime.Parse(regristrationDate);
        }

        public int getEngineSize()
        {
            return this.engineSize;
        }

        public void setEngineSize(int engineSize)
        {
            this.engineSize = engineSize;
        }

        public int getOwnerId()
        {
            return this.ownerId;
        }

        public void setOwnerId(int ownerId)
        {
            this.ownerId = ownerId;
        }


        public Vehicule(int vehiculeId,string manufacturer, string model, string regristrationNumber, string regristrationDate, int engineSize, int ownerId,string vehiculeType,string interiorColour, string hasHelmetCase)
        {
            this.vehiculeId = vehiculeId;
            this.manufacturer = manufacturer;
            this.model = model;
            this.regristrationNumber = regristrationNumber;
            this.regristrationDate = DateTime.Parse(regristrationDate);
            this.engineSize = engineSize;
            this.ownerId = ownerId;
            this.vehiculeType = vehiculeType;
            this.interiorColour = interiorColour;
            this.hasHelmetCase = hasHelmetCase;
        }
    }
}
