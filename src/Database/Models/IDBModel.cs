namespace Database
{
    interface IDBModel
    {
        public string Name { get; set; }

        public long GetId();
    }
}
