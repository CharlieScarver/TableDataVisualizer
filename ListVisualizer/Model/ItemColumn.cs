namespace ListVisualizer.Model
{
    public class ItemColumn
    {
        public ItemColumn() { }

        public ItemColumn(string name, bool isVisible)
        {
            Name = name;
            IsVisible = isVisible;
        }

        public string Name { get; set; }

        public bool IsVisible { get; set; }
    }
}
