using System;
using Grpc.Net.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using System.Collections;
using System.Linq;
using GrpcProtoLib.Protos;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(
            //    new HelloRequest { Name = "GreeterClient" });

            //var result = "Greeting: " + reply.Message;



            var client = new Courses.CoursesClient(channel);

            var reply = await client.CreateCourseAsync(
                new CourseMessage { Title = "BestTitle" });

            var result = reply.Title;

            Console.WriteLine(result);


            Console.WriteLine("Press any key to exit...");



            Console.ReadKey();
        }
    }


}
