using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_253502_Chertok.Models
{
    public class IndexViewModel
    {
        public int SelectedId { get; set; }
        public SelectList ListItems { get; set; }
    }
}
