using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SCRIPTS
{
    [Serializable]
    public class BateauÀPlacer
    {
        public GameObject BateauCube;
        public GameObject BateauPrefab;
        //doive être public
        public int NombreÀPlacer = 1;
        //[HideInInspector]
        //public int NombrePlacer = 0;
    }
}
