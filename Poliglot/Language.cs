using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliglot
{
    class Language {
        int id;
        string name;
        int level;
        string sample;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int Level {
            get { return level; }
            set { level = value; }
        }

        public string Sample{
            get { return sample; }
            set { sample = value; }
        }


    }
}
