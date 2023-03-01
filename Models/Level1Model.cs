using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Level1Model
    {
        public string TagResults(int Tag)
        {
            string TagRes;
            NSNEL2Server.L2ServiceClient client = new NSNEL2Server.L2ServiceClient("NetTcpBinding_IL2Service");
            TagRes = client.OPCTagResults(Tag);
            client.Close();
            return TagRes;
        }
    }
}
