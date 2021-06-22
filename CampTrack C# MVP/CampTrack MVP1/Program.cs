using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace CampTrack_MVP1
{
    class Program
    {
        public static int Count { get; set; }

        static void Main(string[] args)
        {

            string[] licensePlates = new string[3];

            bool parked = true;
            //string licenseplate = "";

            Database database = new Database();

            Console.Title = "CampTrack Console";
            SerialPort myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = "COM3";
            myport.Open();

            while (true)
            {
                string data_rx = myport.ReadLine();
                //Console.WriteLine("Serial Read:" + data_rx);                                         //Writeline voor debugging

                //Getting Name from Database
                string CardNumber = "CardNumber:";
                if (data_rx.IndexOf(CardNumber) != -1)
                {
                    Count = 0;

                    int CardNumberLength = CardNumber.Length;
                    string cardNumberID = data_rx.Substring(CardNumberLength);
                    cardNumberID = cardNumberID.Trim();
                    Console.WriteLine("'" + cardNumberID + "'");
                    string fullName = database.CheckName(cardNumberID);
                    myport.Write("Client:" + fullName);


                    //Getting Client Information
                    string clientID = database.LoadClientID(cardNumberID);

                    //Getting count of objects
                    int count = database.LoadObjectCount(clientID);
                    //string[] licensePlates = new string[count];
                    //LicensePlates = licensePlates;
                    //Getting licenseplates and status

                    Console.WriteLine("'" + clientID + "'");

                    licensePlates = database.LoadLicensePlate(clientID, count);
                    parked = database.LoadParked(clientID);

                    myport.Write("LicensePlate:" + licensePlates[Count]);
                    myport.Write("State:" + parked);

                    //Debug Corner
                    Console.WriteLine("");
                    Console.WriteLine("Fabian's Debug Corner:");
                    Console.WriteLine("----------------------");
                    Console.WriteLine("Cardnumber: " + cardNumberID);
                    Console.WriteLine("Vehicles: " + count);
                    Console.WriteLine("clientid: " + clientID);
                    Console.WriteLine("LicensePlate: " + licensePlates[0]);
                    Console.WriteLine("State: " + parked);
                }

                string option = "Option:";
                if (data_rx.IndexOf(option) != -1)
                {
                    int optionLength = option.Length;
                    string state = data_rx.Substring(optionLength);
                    state = state.Trim();
                    Console.WriteLine(state);

                    if (state.Equals("Changed"))
                    {
                        string objectid = database.LoadObjectID(licensePlates[Count]);


                        if (parked == true)
                        {
                            database.UpdateParking(false, objectid);
                        }
                        if (parked == false)
                        {
                            database.UpdateParking(true, objectid);
                        }

                    }
                }
                string nextPlate = "Next";
                if (data_rx.IndexOf(nextPlate) != -1)
                {
                    Count = Count + 1;

                    if (Count == licensePlates.Length)
                    {
                        Count = 0;
                    }

                   
                    Console.WriteLine(licensePlates[Count]);
                    myport.Write("LicensePlate:" + licensePlates[Count]);

                    


                }



            }
        }
    }
}
