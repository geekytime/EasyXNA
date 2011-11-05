
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EasyXNA
{
    public class Room
    {
        EasyTopDownGame game;
        Dictionary<char, Func<EasyGameComponent>> functionMap;
        Dictionary<char, string> nameMap;

        public Room(EasyTopDownGame game)
        {
            this.game = game;
            this.functionMap = new Dictionary<char, Func<EasyGameComponent>>();
            this.nameMap = new Dictionary<char, string>();
        }

        public void LoadRoomFile(string fileName)
        {
            string filePath = Path.Combine(game.Content.RootDirectory, fileName);

            string lineOfText;
            StreamReader sr = new StreamReader(filePath);
            int rowCount = 0;
            int colCount = 0;
            while ((lineOfText = sr.ReadLine()) != null)
            {
                List<char> chars = lineOfText.ToList<char>();
                colCount = 0;
                chars.ForEach(delegate(char character)
                {
                    handleCharacter(character, rowCount, colCount);
                    colCount++;
                });
                rowCount++;
            }
        }

        private void handleCharacter(char character, int row, int col)
        {
            if (functionMap.ContainsKey(character))
            {
                Func<EasyGameComponent> handler = functionMap[character];
                EasyGameComponent component = handler();
                if (component != null)
                {
                    //component.MoveToGrid(col, row);
                }
            }
            else if (nameMap.ContainsKey(character))
            {
                string componentType = nameMap[character];
                EasyGameComponent component = game.AddComponent(componentType);
                //component.MoveToGrid(col, row);
                component.Static = true;
            }
        }

        public void addCharacterHandler(char character, Func<EasyGameComponent> handler)
        {
            this.functionMap[character] = handler;
        }

        public void addCharacterHandler(char character, string componentType)
        {
            this.nameMap[character] = componentType;
        }
    }
}
