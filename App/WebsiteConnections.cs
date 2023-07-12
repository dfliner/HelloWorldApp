using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldApp
{
    public interface IWebsiteConnections
    {
        IList<string> WebSites { get; }
        Task ConnectInSequentialAsync(string protocol);
        Task ConnectInParallelAsync(string protocol);
    }

    public abstract class WebsiteConnectionsBase
    {
        protected readonly IList<string> websites;

        public WebsiteConnectionsBase(IList<string> websites)
        {
            this.websites = websites;
        }

        public IList<string> Websites => websites;
    }


    /// <summary>
    /// Manages http connections to websites.
    /// </summary>
    public class WebsiteConnections : WebsiteConnectionsBase, IWebsiteConnections
    {
        public IList<string> WebSites => base.Websites;

        //private readonly IList<string> websites;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteConnections"/> class, to manage http
        /// connections to the specified websites.
        /// </summary>
        /// <param name="websites">The list of websites.</param>
        public WebsiteConnections(IList<string> websites)
            : base(websites)
        {
            //this.websites = websites;
        }

        /// <summary>
        /// Connects to websites in sequential order. It is guaranteed that websites are connected
        /// sequentially based upon their index order.
        /// </summary>
        /// <param name="protocol">The protocol used to connect to the websites.</param>
        /// <returns></returns>
        public async Task ConnectInSequentialAsync(string protocol)
        {
            Console.WriteLine("Connecting to websites in sequential order");

            for (int i = 0; i < websites.Count; i++)
            {
                using var client = new HttpClient();
                var result = await client.GetAsync($"{protocol}://{websites[i]}");
                Console.WriteLine($"Connection {i} - {websites[i]}: {result.StatusCode}");
            }
        }

        /// <summary>
        /// Connects to websites in parallel order. All connections are parallelized and there is no
        /// guarantee that websites are connected sequentially in index order.
        /// </summary>
        /// <param name="protocol">The protocol used to connect to the websites.</param>
        /// <returns></returns>
        public async Task ConnectInParallelAsync(string protocol)
        {
            Console.WriteLine("Connecting to webstes in parallel order");

            IList<Task> tasks = new List<Task>();

            for (int i = 0; i < websites.Count; i++)
            {
                int index = i;
                string website = websites[i];

                var task = Task.Run(async () =>
                {
                    using var client = new HttpClient();
                    var result = await client.GetAsync($"{protocol}://{website}");
                    Console.WriteLine($"Connection {index} - {website}: {result.StatusCode}");
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray());
        }
    }
}
