using System;
using System.Collections.Generic;
using System.Text;

namespace TwistecData.Repositories.Interface
{
    public interface ICSVReader
    {

        void Open();
        void Close();
        string[] Next();
    }
}
