using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Play.Catalog.Service.Serializers
{
    public class DateTimeOffsetSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
           return type == typeof(DateTimeOffset) ? new DateTimeOffsetSerializer(BsonType.String) : null;
        }
    }
}