using UnityEngine;

namespace LudumDare51
{
    public class DataKeeper : MonoBehaviour
    {
        private static DataKeeper _instance;
        public static DataKeeper Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("Data Keeper", typeof(DataKeeper));
                    _instance = go.GetComponent<DataKeeper>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        public int FinalScore { set; get; }
    }
}
