using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public static class XML_Reader
{
    public static void SerializeInv(Investigator[] invs)
    {
        XmlSerializer invSerializer = new XmlSerializer(typeof(Investigator[]));
        using (FileStream fs = new FileStream("H:\\Projects/EldritchHorror/Assets/XMLs/investigator.xml", FileMode.OpenOrCreate))
        {
            invSerializer.Serialize(fs, invs);
        }
    }
    public static Ancient DeserializeAncient(string name)
    {
        Ancient? ancient = new Ancient();
        XmlSerializer ancientSerializer = new XmlSerializer(typeof(Ancient[]));
        using (FileStream fs = new FileStream("H:\\Projects/EldritchHorror/Assets/XMLs/ancient.xml", FileMode.OpenOrCreate))
        {
            Ancient?[] ancients = ancientSerializer.Deserialize(fs) as Ancient[];
            foreach (Ancient a in ancients)
            {
                if (a.name == name)
                    ancient = a;
            }
        }
        return ancient;
    }

    public static Investigator DeserializeInvestigator(string name)
    {
        Investigator? investigator = new Investigator();
        XmlSerializer invSerializer = new XmlSerializer(typeof(Investigator[]));
        using (FileStream fs = new FileStream("H:\\Projects/EldritchHorror/Assets/XMLs/investigator.xml", FileMode.OpenOrCreate))
        {
            Investigator?[] investigators = invSerializer.Deserialize(fs) as Investigator[];
            foreach (Investigator a in investigators)
            {
                if (a.Class == name)
                    investigator = a;
            }
        }
        return investigator;
    }
}

