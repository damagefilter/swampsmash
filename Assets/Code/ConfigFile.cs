using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// A simple PropertiesFile implementation.
/// Reads/writes properties files from the unity application path.
/// </summary>
using System;


class ConfigFile {
    private Dictionary<string, string> data;
    private Dictionary<string, object> dataCache;

    private string fileName;

    //public ConfigFile() {
    //    data = new Dictionary<string, string>();
    //    dataCache = new Dictionary<string, object>();
    //    this.fileName = Application.persistentDataPath + "/ERROR.cfg";
    //}

    public ConfigFile(string fileName) {
        data = new Dictionary<string, string>();
        dataCache = new Dictionary<string, object>();
        this.fileName = Application.persistentDataPath + "/" + fileName + ".cfg";
        ReadFile();
    }

    private void ReadFile() {
        if (!File.Exists(this.fileName)) {
            // make empty and get out
            File.Create(this.fileName).Dispose();
            return;
        }
        // Make sure to release the file after using it
        using (StreamReader file = new StreamReader(this.fileName)) {
            string row = null;
            while ((row = file.ReadLine()) != null) {
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            }
        }
    }

    public int GetInt(string name) {
        if (!data.ContainsKey(name)) {
			throw new ArgumentException(name + " does not exist in config " + this.fileName + " and no default was given");
        }
        if (dataCache.ContainsKey(name)) {
            return (int)dataCache[name];
        }
        int tmp = -1;
        if (int.TryParse(data[name], out tmp)) {
            dataCache[name] = tmp;
            return tmp;
        }
		throw new ArgumentException(name + " is not parsable as integer in " + this.fileName + " and no default was given. It is: " + data[name]);
    }

    public int GetInt(string name, int def) {
        if (!data.ContainsKey(name)) {
            dataCache[name] = def;
            data[name] = def.ToString();
        }
        if (dataCache.ContainsKey(name)) {
            return (int)dataCache[name];
        }
        int tmp = -1;
        if (int.TryParse(data[name], out tmp)) {
            dataCache[name] = tmp;
            return tmp;
        }
        return def;
    }

    public void SetInt(string name, int val) {
        data[name] = val.ToString();
        dataCache[name] = val;
    }

    public string GetString(string name) {
        if (!data.ContainsKey(name)) {
			throw new ArgumentException(name + " does not exist in config " + this.fileName + " and no default was given");
        }
        // No caching needed.
        return data[name];
    }

    public string GetString(string name, string def) {
        if (!data.ContainsKey(name)) {
            data[name] = def;
        }
        // No caching needed.
        return data[name];
    }

    public string SetString(string name, string def) {
        // No caching needed.
        return data[name] = def;
    }

    /// <summary>
    /// Contains this key?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(string key) {
        return data.ContainsKey(key);
    }

    /// <summary>
    /// Contains all keys?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(params string[] key) {
        foreach (string name in key) {
            if (!data.ContainsKey(name)) {
                return false;
            }
        }
        return true;
    }

    public void Save() {

        if (!File.Exists(this.fileName)) {
            File.Create(this.fileName).Dispose();
        }

        using (StreamWriter file = new StreamWriter(this.fileName)) {
            foreach (var kvp in data) {
                file.WriteLine(kvp.Key + "=" + kvp.Value);
            }
        }
    }
}
