using System;
using System.Data;
using System.Threading;
using System.Text;
using NetMQ;
using NetMQ.Sockets;
using System.Runtime.InteropServices;


public class Publisher {
    public static void Main() {
        Thread publisher = new Thread(new ThreadStart(sendData));
       
        publisher.Start();
   }   

public static void sendData(){               

        Random rand = new Random();   
        int lenHeader = 5;  
        byte[] bytes = new byte[5];       

        using (var pubSocket = new PublisherSocket()){

            pubSocket.Options.SendHighWatermark = 1000;
            pubSocket.Bind("tcp://127.0.0.1:12345");     
            Thread.Sleep(200);      

            Console.WriteLine("\nİmza\tKaynak\tHedef\tPaket ID  Proje ID   Delay (ms)\n");           

            while(true){
                                                
                int[] delay = {10, 20, 30, 40, 50};
                 
                int ranDelay = rand.Next(0, lenHeader);                 
                byte ranByte1 = Convert.ToByte(rand.Next(1, lenHeader + 1));   
                byte ranByte2 = Convert.ToByte(rand.Next(1, lenHeader + 1));
                byte ranByte3 = Convert.ToByte(rand.Next(1, lenHeader + 1));
                byte ranByte4 = Convert.ToByte(rand.Next(1, lenHeader + 1));
                //byte ranByte4 = 1;
                byte ranByte5 = Convert.ToByte(rand.Next(1, lenHeader + 1));
                         
                bytes[0] = ranByte1;
                bytes[1] = ranByte2;
                bytes[2] = ranByte3;
                bytes[3] = ranByte4;
                bytes[4] = ranByte5;    

                string strDelay = Convert.ToString(delay[ranDelay]);           

                Thread.Sleep(delay[ranDelay]);  
                OutgoingSocketExtensions.SendFrame(pubSocket, bytes);   

                Console.WriteLine(ranByte1 + "\t " + ranByte2 + "\t " + ranByte3 + "\t  " 
                                  + ranByte4  + "\t    " + ranByte5 + "\t\t" + strDelay);
                     
            }
        }   
    }
}
