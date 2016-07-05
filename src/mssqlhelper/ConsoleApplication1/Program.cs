using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mssqlhelper;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<object> result =  SqlHelper.Invoke(new Parms("select * from 系统用户",ExecuteType.Query));
            result.ConfigureAwait(false).GetAwaiter().OnCompleted(()=> 
            {
                Console.WriteLine(result.Result.ToString());
            });
            
            
            Console.Read();
        }
    }
}
