using System;
using Grpc.Net.Client;
using GrpcGreeter;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using System.Collections;
using GrpcGreeter.Protos;
using System.Linq;

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



            var client = new CoursesService.CoursesServiceClient(channel);

            var reply = await client.CreateCourseAsync(
                new CourseRequest { Title = "BestTitle" });

            var result = reply.Courses.FirstOrDefault()?.Title;

            Console.WriteLine(result);


            Console.WriteLine("Press any key to exit...");



            Console.ReadKey();
        }
    }


}
