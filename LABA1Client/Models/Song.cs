using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA1Client.Models
{
    public class Song
    {
        public string song_title { get; set; }
        public string album_title { get; set; }
        public string artist_name { get; set; }
        public TimeSpan song_duration { get; set; }

    }
}
