namespace Voxelity.AddOns.Editor
{
    [System.Serializable]
    public class AddOnInfo
    {
        public string packName;
        public AddOnVersion version;
        public string description;
        public string packPath;
        public bool interactiveImport = true;

        public AddOnInfo(string packName, AddOnVersion version, string description, string packPath, bool interactiveImport)
        {
            this.packName = packName;
            this.version = version;
            this.description = description;
            this.packPath = packPath;
            this.interactiveImport = interactiveImport;
        }
    }
    [System.Serializable]
    public struct AddOnVersion
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        public AddOnVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public static bool operator >(AddOnVersion val1, AddOnVersion val2)
        {
            return val1.Major > val2.Major || (val1.Major == val2.Major && val1.Minor > val2.Minor) || (val1.Major == val2.Major && val1.Minor == val2.Minor && val1.Patch > val2.Patch);
        }

        public static bool operator <(AddOnVersion val1, AddOnVersion val2)
        {
            return val1.Major < val2.Major || (val1.Major == val2.Major && val1.Minor < val2.Minor) || (val1.Major == val2.Major && val1.Minor == val2.Minor && val1.Patch < val2.Patch);
        }

        public AddOnVersion IncreaseMajor(int index = 1)
        {
            return new AddOnVersion(Major + index, Minor, Patch);
        }

        public AddOnVersion IncreaseMinor(int index = 1)
        {
            return new AddOnVersion(Major, Minor + index, Patch);
        }

        public AddOnVersion IncreasePatch(int index = 1)
        {
            return new AddOnVersion(Major, Minor, Patch + index);
        }
    }

}
