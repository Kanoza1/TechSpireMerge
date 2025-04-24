using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image1 { get; set; }  
        public string Image2 { get; set; }  
        public DateTime PublishDate { get; set; }
    }
}
