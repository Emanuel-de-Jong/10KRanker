using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10KRanker
{
    public enum InputType
    {
        Link,
        Id,
        Name
    }


    public static class Validator
    {
        public static InputType GetMapInputType(string input)
        {
            if (input.Contains("osu.ppy.sh"))
                return InputType.Link;

            if (long.TryParse(input, out long _))
                return InputType.Id;

            return InputType.Name;
        }

        public static InputType GetUserInputType(string input)
        {
            return GetMapInputType(input);
        }

        public static long MapLinkToId(string link)
        {
            string before = "sets/";
            int i = link.IndexOf(before);
            if (i == -1)
                throw new ArgumentException("The map link is not valid.");

            link = link.Substring(i + before.Length);

            i = link.IndexOf("#");
            if (i != -1)
                link = link.Substring(0, i);

            return StringToId(link);
        }

        public static long UserLinkToId(string link)
        {
            string before = "users/";
            int i = link.IndexOf(before);
            if (i == -1)
                throw new ArgumentException("The user link is not valid.");

            link = link.Substring(i + before.Length);

            i = link.IndexOf("/");
            if (i != -1)
                link = link.Substring(0, i);

            return StringToId(link);
        }

        public static long StringToId(string str)
        {
            if (long.TryParse(str, out long id))
                return id;

            throw new ArgumentException("The id in the link has other characters than just numbers.");
        }
    }
}
