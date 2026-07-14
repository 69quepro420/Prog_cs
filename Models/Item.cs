namespace Prog_cs.Models
{
    public class Item
    {
        public string Nome { get; set; }
        public Item(string nome) => Nome = nome;
        public override string ToString() => Nome;
    }
}
