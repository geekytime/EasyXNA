using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace EasyXNA
{
    public class PhysicsFrame
    {
        public List<Vertices> Vertices { get; set; }
        public Vector2 Offset { get; set; }
        public List<Fixture> Fixtures { get; set; }

        public PhysicsFrame(List<Vertices> vertices, Vector2 offset)
        {
            Vertices = vertices;
            Offset = offset;
        }

        public void EnableFixtures(){
            Fixtures.ForEach(fixture=> fixture.CollidesWith = Category.All);
        }

        public void DisableFixtures()
        {
            Fixtures.ForEach(fixture => fixture.CollidesWith = Category.None);
        }
    }
}
