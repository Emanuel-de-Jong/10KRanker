﻿namespace Database.Models
{
    public class Nominator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Nominator(string name)
        {
            Name = name;
        }
    }
}