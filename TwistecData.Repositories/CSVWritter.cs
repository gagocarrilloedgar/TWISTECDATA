using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwistecData.Repositories.Interface;

namespace TwistecData.Repositories
{
    public class CSVWritter:ICSVWritter
    {

        #region Properties
        public string Path;
        private string Dataset_Sep;//symbol that represent the new set of data
        public string[] Headers; //store the csv header
        private StreamWriter Writer;
        #endregion

        #region Constructors
        public CSVWritter(string path)
        {
            Path = path;
            Dataset_Sep = System.Environment.NewLine;
        }

       /* public CSVWritter(string path, string[] headers)
        {
            Path = path;
            Headers = headers;
            Dataset_Sep = System.Environment.NewLine;
        }*/

        #endregion

        #region Methods

        #region Open
        /// <summary>
        /// Open file
        /// </summary>
        public void Open()
        {
            try
            {
                //create streamwriter append mode
                Writer = new StreamWriter(Path, true);
            }
            catch
            {
                throw (new Exception("Error opening/creating file"));
            }
        }
        #endregion

        #region Close
        /// <summary>
        /// Close file
        /// </summary>
        public void Close()
        {
            try
            {
                Writer.Close();
            }
            catch
            {
                throw (new Exception("Error closing file"));
            }
        }
        #endregion

        #region Public Write
        /// <summary>
        /// Write parameters to file in order, and insert a new line
        /// when finished
        /// </summary>
        /// <param name="paramaters"></param>
        public void Write(string[] paramaters)
        {
            try
            {
                write(paramaters);
            }
            catch
            {
                throw (new Exception("Error writing to file"));
            }
        }
        #endregion

        #region Write Header
        /// <summary>
        /// Write headers into file
        /// </summary>
        /// <param name="header"></param>
        public void WriteHeader(string[] header)
        {
            Headers = header;
            write(header);
        }
        #endregion

        #region Private Write
        /// <summary>
        /// Private write method
        /// </summary>
        /// <param name="paramaters"></param>
        private void write(string[] paramaters)
        {
            for (int i = 0; i < paramaters.Length; i++)
            {
                //Write parameter
                Writer.Write(paramaters[i]);
                if (i != paramaters.Length - 1)
                {
                    //write comma
                    Writer.Write(",");
                }
                else
                {
                    //last parameter, dont write comma
                    break;
                }
            }
            Writer.Write(Dataset_Sep);
        }
        #endregion

        #endregion
    }
}
