using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwistecData.Repositories.Interface;

namespace TwistecData.Repositories
{
    public class CSVReader:ICSVReader
    {

        #region Properties
        public string Path; //file path
        private string Dataset_Sep;//symbol that represent the new set of data
        private StreamReader Reader; //class for read files
        public string[] Headers; //store the csv header
        public bool StoreHeaders;
        private int Param_perLine; //the numbers of parameters per line
        private string[] Values;   //store the csv parameters per line
        private bool firstTime_flag; //determine if it is the first time reading file
        #endregion

        #region Constructors
        public CSVReader(string path)
        {
            Path = path;
            StoreHeaders = false;
            Param_perLine = 0;
            Dataset_Sep = System.Environment.NewLine;
            firstTime_flag = true;
        }

       /* public CSVReader(string path, bool storeheaders)
        {
            Path = path;
            StoreHeaders = storeheaders;
            Param_perLine = 0;
            Dataset_Sep = System.Environment.NewLine;
            firstTime_flag = true;
        }*/

        /*public CSVReader(string path, string dataset_sep, bool storeheaders)
        {
            Path = path;
            StoreHeaders = storeheaders;
            Param_perLine = 0;
            Dataset_Sep = dataset_sep;
            firstTime_flag = true;
        }*/
        #endregion

        #region Methods

        #region Open
        public void Open()
        {
            try
            {
                Reader = new StreamReader(Path);
            }
            catch
            {
                throw new Exception("Error opening csv file");
            }
        }
        #endregion

        #region Close
        public void Close()
        {
            try
            {
                Reader.Close();
            }
            catch
            {
                throw (new Exception("Error closing file"));
            }
        }
        #endregion

        #region Next
        /// <summary>
        /// Gets the next parameters of csv file as string array
        /// </summary>
        /// <returns></returns>
        public string[] Next()
        {
            #region Reading file first time
            if (firstTime_flag)
            {
                //reading file first time
                string line = getLine();
                Param_perLine = getNumbers_OfParams_perLine(line);
                if (StoreHeaders)
                {
                    //store headers
                    Headers = new string[Param_perLine - 1];
                    Headers = getParams(line);
                    firstTime_flag = false;
                    return Headers;
                }
                else
                {
                    //do not store headers
                    Values = new string[Param_perLine - 1];
                    Values = getParams(line);
                    firstTime_flag = false;
                    return Values;
                }

            }
            #endregion

            #region not reading first time
            else
            {
                string line = getLine();
                if (line != null)
                {
                    Values = getParams(line);
                    return Values;
                }
                else
                {
                    return null;
                }
            }
            #endregion
        }
        #endregion

        #region getParams
        /// <summary>
        /// Get the paramaters of the line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string[] getParams(string line)
        {
            string[] values = new string[Param_perLine];
            for (int i = 0; i < Param_perLine; i++)
            {
                values[i] = getParam(line, i);
            }
            return values;
        }
        #endregion

        #region getLine
        /// <summary>
        /// Get line of the csv file
        /// </summary>
        /// <returns></returns>
        private string getLine()
        {
            return (Reader.ReadLine());
        }
        #endregion

        #region getNumbers_OfParams_perLine
        /// <summary>
        /// Get the number of the paramaters per line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public int getNumbers_OfParams_perLine(string line)
        {
            //get the values per line
            //the values per line are determined by the numbers of commas
            int number = 0;
            int startIndex = 0;
            int index = 0;
            while (index != -1)
            {
                index = line.IndexOf(",", startIndex);
                if (index != -1)
                {
                    //comma founded
                    //one value
                    number++;
                    startIndex = index + 1;
                }
            }
            return number + 1;
        }
        #endregion

        #region getParam
        /// <summary>
        /// Get specified parameter
        /// </summary>
        /// <param name="line">The line where params are</param>
        /// <param name="paramIndex">The index of param in line</param>
        /// <returns></returns>
        private string getParam(string line, int paramIndex)
        {
            //get commas index
            int[] commasIndex = getCommas_index(line);
            //get the param specified in paramIndex
            #region get the first param
            if (paramIndex == 0)
            {
                return (getFirstParam(line, commasIndex));
            }
            #endregion

            #region get the last param
            if (paramIndex == Param_perLine - 1)
            {
                return (getLastParam(line, commasIndex));
            }
            #endregion

            #region get intermediate param
            else if (paramIndex < Param_perLine && paramIndex > 0)
            {
                return (getIntermediateParam(line, paramIndex, commasIndex));
            }
            throw (new Exception("Wrong Param Index"));
            #endregion
        }

        #region get first param
        private string getFirstParam(string line, int[] commasIndex)
        {
            //get the first param
            string param = line.Substring(0, commasIndex[0]);
            return param;
        }
        #endregion

        #region get the last param
        private string getLastParam(string line, int[] commasIndex)
        {
            //get the last param
            string param = line.Substring(commasIndex[commasIndex.Length - 1] + 1);
            return param;
        }

        #endregion

        #region get intermediate param
        private string getIntermediateParam(string line, int paramIndex, int[] commasIndex)
        {
            //get intermediate param
            int length = commasIndex[paramIndex] - commasIndex[paramIndex - 1] - 1;
            string param = line.Substring(commasIndex[paramIndex - 1] + 1,
                length);
            return param;
        }
        #endregion

        #endregion

        #region Get commas index
        /// <summary>
        /// Get the commas index
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private int[] getCommas_index(string line)
        {
            //search the commas index
            int[] commas_index = new int[Param_perLine - 1];
            int index = 0;
            int commas_startIndex = 0;
            int counter = 0;
            while (index != -1)
            {
                index = line.IndexOf(",", commas_startIndex);
                if (index != -1)
                {
                    //comma founded
                    commas_index[counter] = index;
                    commas_startIndex = index + 1;
                    counter++;
                }
            }
            return commas_index;
        }
        #endregion

        #endregion

    }
}
