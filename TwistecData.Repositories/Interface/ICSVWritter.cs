using System;
using System.Collections.Generic;
using System.Text;

namespace TwistecData.Repositories.Interface
{
    public interface ICSVWritter
    {
        void Open();
        void Close();
        void Write(string[] paramaters);
        void WriteHeader(string[] header);
    }
}
