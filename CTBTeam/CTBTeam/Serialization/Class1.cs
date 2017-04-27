using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CTBTeam.Serialization
{
    [Serializable()]
    public class Hours : ISerializable
    {
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    class Employee
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public List hours;
    }
}