using System;
using System.Data;
using System.Threading;
using System.Text;
using NetMQ;
using NetMQ.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;



public class Publisher {
    public static void Main() {
        Thread publisher = new Thread(new ThreadStart(sendData));
       
        publisher.Start();

        //Console.WriteLine("Hello World");
   }   


public static byte[] EncryptAes(byte[] data)
{
    byte[] Key = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20 };
    byte[] IV = new byte[] { 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30 };
    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = Key;
        aesAlg.IV = IV;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                csEncrypt.Write(data, 0, data.Length);
                csEncrypt.FlushFinalBlock();
                return msEncrypt.ToArray();
            }
        }
    }
}
public static void sendData(){               

        Random rand = new Random();   
        int lenHeader = 5;  
             

        using (var pubSocket = new PublisherSocket()){

            pubSocket.Options.SendHighWatermark = 1000;
            pubSocket.Bind("tcp://127.0.0.1:12345");     
            Thread.Sleep(200);      

            Console.WriteLine("\nİmza\tKaynak\tHedef\tPaket ID  Proje ID   Delay (ms)\n");           
            int count = 0;
            while(true){

                count++;
                int length = rand.Next(70,1000);   
                byte[] bytes = new byte[length];                               
                int[] delay = {1, 1, 1, 1, 1};
                 
                int ranDelay = rand.Next(0, lenHeader);                 
                byte ranByte1 = Convert.ToByte(rand.Next(0, 15));  
                byte ranByte2 = 0;
                switch (ranByte1)
                {
                    case 0:
                        ranByte2 = Convert.ToByte(rand.Next(0,9));
                        break;
                    case 1:
                        ranByte2 = Convert.ToByte(rand.Next(0,10));
                        break;
                    case 2:
                        ranByte2 = Convert.ToByte(rand.Next(0,5));
                        break;
                    case 3:
                        ranByte2 = Convert.ToByte(rand.Next(0,5));
                        break;
                    case 4:
                        ranByte2 = Convert.ToByte(rand.Next(0,5));
                        break;                        
                    case 5:
                        ranByte2 = Convert.ToByte(rand.Next(0,10));
                        break;
                    case 6:
                        ranByte2 = Convert.ToByte(rand.Next(0,8));
                        break;
                    case 7:
                        ranByte2 = Convert.ToByte(rand.Next(0,13));
                        break;
                    case 8:
                        ranByte2 = Convert.ToByte(rand.Next(0,6));
                        break;
                    case 9:
                        ranByte2 = Convert.ToByte(rand.Next(0,7));
                        break;
                    case 10:
                        ranByte2 = Convert.ToByte(rand.Next(0,4));
                        break;
                    case 11:
                        ranByte2 = Convert.ToByte(rand.Next(0,7));
                        break;
                    case 12:
                        ranByte2 = Convert.ToByte(rand.Next(0,9));
                        break;
                    case 13:
                        ranByte2 = Convert.ToByte(rand.Next(0,5));
                        break;
                    case 14:
                        ranByte2 = Convert.ToByte(rand.Next(0,8));                                            
                        break;
                } 
                //ranByte2 = 0;
                //byte ranByte2 = Convert.ToByte(rand.Next(0, lenHeader));
                byte ranByte3 = Convert.ToByte(rand.Next(0, lenHeader + 5));
                byte ranByte4 = Convert.ToByte(rand.Next(0, lenHeader + 5));
                //byte ranByte4 = 1; + 4+1+4 + 5
                byte ranByte5 = Convert.ToByte(rand.Next(0, lenHeader + 5));
                         
                bytes[0] = ranByte1;
                bytes[1] = ranByte2;
                bytes[2] = ranByte3;
                bytes[3] = ranByte4;
                bytes[4] = ranByte5;

                bytes = EncryptAes(bytes);

                string strDelay = Convert.ToString(delay[ranDelay]);           

                Thread.Sleep(delay[ranDelay]);  
                OutgoingSocketExtensions.SendFrame(pubSocket, bytes);   

                Console.WriteLine(ranByte1 + "\t " + ranByte2 + "\t " + ranByte3 + "\t  " 
                                  + ranByte4  + "\t    " + ranByte5 + "\t\t" + strDelay);
                     
            }
        }   
    }
}
