using System;
namespace Utils
{
	public class CmdLine
	{
        private Dictionary<String, String> argDict;

        public CmdLine(string[] args)
		{
            argDict = new Dictionary<String, String>();

            if (args != null && args.Length > 0)
            {
                var argList = new List<string>(args);
                var argKvList = new List<KeyValuePair<String, String>>();
                argList.ForEach(itm => argKvList.Add(ArgToKeyValuePair(itm)));
                argDict = argKvList.ToDictionary(itm => itm.Key, itm => itm.Value);
            }
        }

        private KeyValuePair<string, string> ArgToKeyValuePair(string arg)
        {
            string[] pair = arg.Split('=');
            string key = "", val = "";

            if (pair.Length > 0)
            {
                key = pair[0].ToLower().Trim();

                while(key.StartsWith("-")) // "---file=<file>"
                {
                    key = key.Substring(1); //Remove starting "-"
                }

                key = key.Trim(); // In case there are additional spaces
            }

            if (pair.Length > 1)
            {
                val = pair[1].Trim();

                // In case ugly formatted Boolean: TrUe/fALsE

                if (val.ToLower() == "true")
                    val = "True";
                else if (val.ToLower() == "false")
                    val = "False";
            }

            return new KeyValuePair<string, string>(key, val);
        }

        public bool FindSwitch(string key)
        {
            return argDict.ContainsKey(key.ToLower());
        }

        public string GetString(string key)
        {
            string retVal = string.Empty;

            if (argDict.ContainsKey(key.ToLower()))
            {
                retVal = argDict[key];
            }

            return retVal;
        }

        public bool GetBool(string key)
        {
            string retVal = GetString(key);
            return Convert.ToBoolean(retVal);
        }

        public int GetNumber(string key)
        {
            string retVal = GetString(key);
            return Convert.ToInt32(retVal);
        }
    }
}

