using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeoPicker
{
    public class DownloadWorkerMarket
    {
        public int TotalWorkerNumber { get; }

        private BlockingCollection<WebClient> _wcs = new BlockingCollection<WebClient>(new ConcurrentStack<WebClient>());

        public DownloadWorkerMarket(int totalWorkerNumber)
        {
            TotalWorkerNumber = totalWorkerNumber;

            for(int i = 0; i< totalWorkerNumber; i++)
            {
                _wcs.Add(new WebClient());
            }
        }

        public WebClient GetOne()
        {
            return _wcs.Take();
        }

        public void Release(WebClient wc)
        {
            _wcs.Add(wc);
        }
    }
}
