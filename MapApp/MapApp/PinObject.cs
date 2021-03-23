using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapApp
{
    public class PinObject : RealmObject
    {
        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IList<String> PhotosPaths { get; }
    }
}
