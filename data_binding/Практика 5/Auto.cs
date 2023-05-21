using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Практика_5
{
    public enum eGroup { Легковые, Грузовые, Мотоциклы };

    [Serializable]
    public class Auto
    {
        [DisplayName("Марка машины"), Category("Сводка")]
        public string AutoName { get; set; }

        [DisplayName("Страна производства"), Category("Сводка")]
        public string Country { get; set; }

        [DisplayName("Год выпуска"), Category("Сводка")]
        public int Year { get; set; }

        [DisplayName("Цена"), Category("Сводка")]
        public int Cost { get; set; }

        [DisplayName("Фото"), Category("Сводка")]
        public string Img { get; set; } = "../../img/default.png";

        [DisplayName("Категория"), Category("Сводка")]
        public eGroup Groups { get; set; }
        [Browsable(false)]
        public string StrGroup
        {
            get
            {
                return Groups.ToString();
            }
            set
            {
                Groups.ToString();
            }
        }
        public Auto(string n, string c, int y, int b, eGroup s, string i)
        {
            AutoName = n;
            Country = c;
            Year = y;
            Cost = b;
            Img = i;
            Groups = s;
        }
        public Auto ()
        {

        }
    }
}
