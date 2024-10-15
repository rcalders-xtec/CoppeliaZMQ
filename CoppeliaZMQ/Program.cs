using System;
using NetMQ;
using NetMQ.Sockets;

class Program
{
    static void Main(string[] args)
    {
        using (var pair = new PairSocket())
        {
            pair.Connect("tcp://localhost:5555");

            // Create a binary message
            byte[] binaryMessage = new byte[] { 0x41, 0x42,0x43 };
            Console.WriteLine("Sending binary message to server...");

            // Send binary data to the server
//            pair.SendFrame(binaryMessage);

            // Receive the reply from the server
 //           var reply = pair.ReceiveFrameBytes();
  //          Console.WriteLine("Received reply: " + BitConverter.ToString(reply));

            // Listen for unsolicited messages
            while (true)
            {
                pair.SendFrame(binaryMessage);

                byte[] unsolicitedMessage = pair.ReceiveFrameBytes();
                Console.WriteLine("Received unsolicited message: " + BitConverter.ToString(unsolicitedMessage));
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
