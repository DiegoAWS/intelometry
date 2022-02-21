
namespace intelometry.Models
{
    public class Filter
    {
      
        public string Field { get; set; }
        public string Comparator { get; set; }
        public string Value { get; set; }

        public Filter(string _Field, string _Comparator, string _Value)
        {
            Value = _Value;
            Comparator = _Comparator;
            Field = _Field;
        }
    }
}
