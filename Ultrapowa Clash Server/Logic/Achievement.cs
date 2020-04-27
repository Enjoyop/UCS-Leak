namespace UCS.Logic
{
    internal class Achievement
    {
        const int m_type = 23000000;

        public int Id => this.m_type + Index;

        public int Index { get; set; }

        public string Name { get; set; }

        public bool Unlocked { get; set; }

        public int Value { get; set; }

        public Achievement()
        {
        }

        public Achievement(int index)
        {
            this.Index = index;
            this.Unlocked = false;
            this.Value = 0;
        }
    }
}
