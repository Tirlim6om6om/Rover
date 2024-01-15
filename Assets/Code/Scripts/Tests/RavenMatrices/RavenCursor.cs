using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ITPRO.Rover.Test.Raven
{
    public class RavenCursor : Cursor
    {
        public RavenAnswer SelectImage { get; private set; }

        public void SetSelectImage(RavenAnswer answer) => SelectImage = answer;

        public void DeselectImage() => SelectImage = null;
    }   
}