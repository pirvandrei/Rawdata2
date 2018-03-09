using System;

namespace DomainModel
{
    public class Request
    {
        public object Method
        {
            get;
            set;
        }
        public string Path
        {
            get;
            set;
        }
        public long Date
        {
            get;
            set;
 
        }

        public string Body
        {
            get;
            set;
        }
    }
}
