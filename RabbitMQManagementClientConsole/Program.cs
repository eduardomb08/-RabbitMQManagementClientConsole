using EasyNetQ.Management.Client;
using System;
using System.Threading.Tasks;

namespace RabbitMQManagementClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var mc = new ManagementClient("http://localhost", "guest", "guest");
            await DeleteExchanges(mc);
            await DeleteQueues(mc);

            Console.Write(".");
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task DeleteExchanges(ManagementClient mc)
        {
            var exchanges = await mc.GetExchangesAsync();

            foreach (var exchange in exchanges)
            {
                if (exchange.Name.StartsWith("System:")
                 || exchange.Name.StartsWith("amq.")
                 || exchange.Name.StartsWith("nsb."))
                    continue;

                Console.WriteLine(exchange.Name);
                await mc.DeleteExchangeAsync(exchange);
            }
        }

        private static async Task DeleteQueues(ManagementClient mc)
        {
            var queues = await mc.GetQueuesAsync();

            foreach (var queue in queues)
            {
                if (queue.Name.StartsWith("nsb."))
                    continue;

                Console.WriteLine(queue.Name);
                await mc.DeleteQueueAsync(queue);
            }
        }
    }
}
