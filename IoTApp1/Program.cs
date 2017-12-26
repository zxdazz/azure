using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading;

namespace IoTApp1
{
    class Program
    {
        static void Main(string[] args)
        {
           // init storage account from app.config
            CloudStorageAccount sa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the CloudBlobClient that is used to call the Blob Service for that storage account.
            CloudBlobClient cloudBlobClient = sa.CreateCloudBlobClient();

            // Create hardcoded container if not exist
            string folder = "processing2";
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(folder);
            container.CreateIfNotExists();

            // upload blob from local folder
            while (true)
            {
                string[] files = Directory.GetFiles(@"D:\temp\");

                foreach (string s in files)
                {
                    string key = Path.GetFileName(s);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(key);
                    File.OpenRead(s).Close();
                    blockBlob.UploadFromFile(s);
                    Console.WriteLine("Uploaded = {0}", s);
                    File.Delete(s);
                }
                int e = files.Length;
                Console.WriteLine("Files uploaded to Azure Storage:" + e);
                Thread.Sleep(1000 * 3);
            }
            //Console.ReadKey();
            
        }
        


        }
    }

