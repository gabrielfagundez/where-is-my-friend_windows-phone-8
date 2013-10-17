using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WhereIsMyFriend.Classes
{
    public struct nodo
    {
        public Color color;
        public GeoCoordinate pos;
        public string id;
        public string name;
    };

    class PointsHandler
    {
        private static PointsHandler instance;
        private Dictionary<string, nodo> pines;
        public GeoCoordinate myPosition { get; set; }



        private PointsHandler()
        {
            pines = new Dictionary<string, nodo>();
        }

        public static PointsHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PointsHandler();
                }
                return instance;
            }
        }



        public void insert(string id, string nom, GeoCoordinate geo)
        {
            nodo n = new nodo();
            n.color = Colors.Red;
            n.id = id;
            n.name = nom;
            n.pos = geo;
            if (pines.ContainsKey(id))
            {
                pines.Remove(id);

            }
            pines.Add(id, n);

        }
        public List<KeyValuePair<string, nodo>> allCoords()
        {
            List<KeyValuePair<string, nodo>> l = pines.ToList();
            return l;
        }

        public void deleteFriend(string id)
        {
            if (pines.ContainsKey(id))
            {
                pines.Remove(id);

            }
        }
    }
}
