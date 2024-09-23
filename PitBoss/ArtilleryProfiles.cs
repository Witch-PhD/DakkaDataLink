namespace PitBoss
{
    public class ArtilleryProfiles
    {
        private static ArtilleryProfiles? m_Instance = null;
        public static ArtilleryProfiles Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ArtilleryProfiles();
                }
                return m_Instance;
            }

            private set { }
        }

        private ArtilleryProfiles()
        {
            ArtyProfilesDict = new Dictionary<string, ArtilleryProfile>
            {
                { Lariat120mm.ProfileName, Lariat120mm },
                { Exalt150mm.ProfileName, Exalt150mm },
                { Skycaller.ProfileName, Skycaller },
                { Cremari_Mortar.ProfileName, Cremari_Mortar },
                { Mortar_Light_Tank.ProfileName, Mortar_Light_Tank },
            };
            CurrentProfile = ArtyProfilesDict["Lariat 120mm"];
        }
        public ArtilleryProfile CurrentProfile { get; set; }

        public void SwitchToProfile(string profileName)
        {
            switch (profileName)
            {
                case LARIAT_120_NAME:
                    CurrentProfile = Lariat120mm;
                    break;

                case EXALT_150_NAME:
                    CurrentProfile = Exalt150mm;
                    break;

                case SKYCALLER_NAME:
                    CurrentProfile = Skycaller;
                    break;

                default:
                    //Console.WriteLine($"ArtilleryProfiles.SwitchToProfile: Invalid profile name received: {profileName}");
                    CurrentProfile = Lariat120mm;
                    break;
            }
        }

        public Dictionary<string, ArtilleryProfile>ArtyProfilesDict { get; set; }

        public readonly ArtilleryProfile Lariat120mm = new ArtilleryProfile(LARIAT_120_NAME, 8.0, 100.0, 300.0);
        public readonly ArtilleryProfile Exalt150mm = new ArtilleryProfile(EXALT_150_NAME, 8.0, 100.0, 300.0);
        public readonly ArtilleryProfile Skycaller = new ArtilleryProfile(SKYCALLER_NAME, 15.0, 200.0, 275.0);
        public readonly ArtilleryProfile Cremari_Mortar = new ArtilleryProfile(CREMARI_MORTAR_NAME, 0.5, 45.0, 80.0);
        public readonly ArtilleryProfile Mortar_Light_Tank = new ArtilleryProfile(MORTAR_LIGHT_TANK_NAME, 2.3, 45.0, 80.0);

        public const string LARIAT_120_NAME = "Lariat 120mm";
        public const string EXALT_150_NAME = "Exalt 150mm";
        public const string SKYCALLER_NAME = "Skycaller";
        public const string CREMARI_MORTAR_NAME = "Cremari Mortar";
        public const string MORTAR_LIGHT_TANK_NAME = "Devitt Mortar Tank (MLT)";

        public class ArtilleryProfile
        {
            public readonly string ProfileName;
            public readonly double DistTickSize;
            public readonly double MinDist;
            public readonly double MaxDist;

            internal ArtilleryProfile(string _profileName, double _distTickSize, double _minDist, double _maxDist)
            {
                ProfileName = _profileName;
                DistTickSize = _distTickSize;
                MinDist = _minDist;
                MaxDist = _maxDist;
            }
        }
    }
}
