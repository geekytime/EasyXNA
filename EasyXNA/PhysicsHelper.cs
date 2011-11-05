using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using SpriteSheetRuntime;
using FarseerPhysics.Dynamics;

namespace EasyXNA
{
    class PhysicsHelper
    {
        public static PhysicsFrame GetVerticesForTexture(SpriteSheet spriteSheet, int index)
        {
            Rectangle spriteSourceRect = spriteSheet.SourceRectangle(index);
            uint[] textureData = GetTextureData(spriteSheet, spriteSourceRect);
            return GetVerticesForTextureData(textureData, spriteSourceRect.Width);
        }

        public static PhysicsFrame GetVerticesForTexture(Texture2D texture)
        {
            uint[] textureData = GetTextureData(texture);
            return GetVerticesForTextureData(textureData, texture.Width);
        }

        public static PhysicsFrame GetVerticesForTextureData(uint[] textureData, int textureWidth)
        {
            Vertices textureVertices = PolygonTools.CreatePolygon(textureData, textureWidth, false);
            Vector2 offset = -textureVertices.GetCentroid();
            textureVertices.Translate(ref offset);            
            SimplifyTools.MergeParallelEdges(textureVertices, 0);
            List<Vertices> convexVertices = BayazitDecomposer.ConvexPartition(textureVertices);
            ConvertUnits.ScaleToSimUnits(ref convexVertices);
            return new PhysicsFrame(convexVertices, -offset);
        }

        public static uint[] GetTextureData(Texture2D texture)
        {
            int dataLength = texture.Width * texture.Height;
            uint[] pixelData = new uint[dataLength];
            texture.GetData<uint>(pixelData);
            return pixelData;
        }

        public static uint[] GetTextureData(SpriteSheet spriteSheet, Rectangle spriteSourceRect)
        {            
            int dataLength = spriteSourceRect.Width * spriteSourceRect.Height;
            uint[] pixelData = new uint[dataLength];
            spriteSheet.Texture.GetData<uint>(0, spriteSourceRect, pixelData, 0, dataLength);
            return pixelData;
        }

        public static Vector2 GetRandomDirectionUnitVector()
        {
            float x = (float)RandomHelper.DoubleInRange(-1, 1);
            float y = (float)RandomHelper.DoubleInRange(-1, 1);
            Vector2 randomVector = new Vector2(x, y);
            randomVector.Normalize();
            return randomVector;
        }

        public static float VectorToRotation(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X) + MathHelper.PiOver2 ;   
        }

        public static void AttachOnCollisionHandlers(Body body, EasyTopDownGame game)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.OnCollision += game.OnFixtureCollision;
            }
        }
    }
}
