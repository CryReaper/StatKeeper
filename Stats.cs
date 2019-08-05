using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace StatKeeper
{
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("Item");

                reader.ReadStartElement("Key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("Value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("Item");

                writer.WriteStartElement("Key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("Value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }

    public class Stats : IDefaultable {
        public int GetKills(string cause)
        {
            if (!Kills.ContainsKey(cause)) return 0;
            return Kills[cause];
        }

        public int GetDeaths(string cause)
        {
            if (!Deaths.ContainsKey(cause)) return 0;
            return Deaths[cause];
        }

        public ulong SteamID { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;

        public SerializableDictionary<string, int> Kills { get; set; } = new SerializableDictionary<string, int>();
        public SerializableDictionary<string, int> Deaths { get; set; } = new SerializableDictionary<string, int>();

        public double KDRatio { get; set; } = 0;
        public int TotalKills { get; set; } = 0;
        public int TotalDeaths { get; set; } = 0;

        public void RecalculateStats()
        {
            TotalDeaths = Deaths.Values.Sum();
            TotalKills = Kills.Values.Sum();
            if(TotalDeaths != 0)
                KDRatio =  TotalKills / TotalDeaths;
            LastUpdated = DateTime.Now;
        }

        public Stats() { }

        public void LoadDefaults()
        {
            SteamID = 0;
            LastUpdated = DateTime.Now;
            Created = DateTime.Now;
            Kills = new SerializableDictionary<string, int>();
            Deaths = new SerializableDictionary<string, int>();
            KDRatio = 0;
            TotalKills = 0;
            TotalDeaths = 0;
        }
    }
}
