namespace UniModules.UniGame.SplitTexture.Editor
{
    using System.Collections.Generic;
    using Abstract;
    using UnityEngine;

    public sealed class UniformTextureSplitter : ITextureSplitter
    {
        public Texture2D[] SplitTexture(Texture2D source, Vector2Int maxSize)
        {
            if (source == null) {
                return null;
            }
            
            if (source.width > maxSize.x && source.height > maxSize.y) {
                return SplitVerticalAndHorizontal(source, maxSize);
            }

            if (source.width > maxSize.x) {
                return SplitHorizontal(source, maxSize.x);
            }

            if (source.height > maxSize.y) {
                return SplitVertical(source, maxSize.y);
            }

            return new[] {source};
        }

        private Texture2D[] SplitHorizontal(Texture2D source, int maxSize)
        {
            var splitCount = SplitHelper.GetSplitCount(source.width, maxSize);
            if (splitCount > 1) {
                var partWidth = Mathf.FloorToInt((float)source.width / splitCount);
                
                var resultArray = new Texture2D[splitCount];
                for (var i = 0; i < splitCount; i++) {
                    var rect = new Rect(i * partWidth, 0.0f, partWidth, source.height);
                    if (i == splitCount - 1) {
                        var commonSplitWidth = partWidth * i;
                        partWidth = source.width - commonSplitWidth;
                        rect = new Rect(commonSplitWidth, 0.0f, partWidth, source.height);
                    }
                    
                    var texture = GetTextureByRect(source, rect);
                    texture.name = SplitHelper.GetSplittedTextureName(i, source.name);
                    resultArray[i] = texture;
                }

                return resultArray;
            }

            return null;
        }

        private Texture2D[] SplitVertical(Texture2D source, int maxSize)
        {
            var splitCount = SplitHelper.GetSplitCount(source.height, maxSize);
            if (splitCount > 1) {
                var partHeight = Mathf.FloorToInt((float)source.height / splitCount);
                
                var resultArray = new Texture2D[splitCount];
                for (var i = 0; i < splitCount; i++) {
                    if (i == splitCount - 1) {
                        var commonSplitHeight = partHeight * i;
                        partHeight = source.height - commonSplitHeight;
                    }

                    var rect = new Rect(0.0f, i * partHeight, source.width, partHeight);
                    var texture = GetTextureByRect(source, rect);
                    texture.name = SplitHelper.GetSplittedTextureName(i, source.name);
                    resultArray[i] = texture;
                }

                return resultArray;
            }

            return null;
        }

        private Texture2D[] SplitVerticalAndHorizontal(Texture2D source, Vector2Int maxSize)
        {
            var splittedByWidth = SplitHorizontal(source, maxSize.x);
            if (splittedByWidth != null) {
                var resultList = new List<Texture2D>();
                foreach (var texture in splittedByWidth) {
                    var splittedByHeight = SplitVertical(texture, maxSize.y);
                    if(splittedByHeight != null) {
                        resultList.AddRange(splittedByHeight);
                    }
                    else {
                        resultList.Add(texture);
                    }
                }

                for (var i = 0; i < resultList.Count; i++) {
                    resultList[i].name = SplitHelper.GetSplittedTextureName(i, source.name);
                }

                return resultList.ToArray();
            }

            return null;
        }

        private Texture2D GetTextureByRect(Texture2D source, Rect rect)
        {
            var resultTexture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
            var pixels = source.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
            
            resultTexture.SetPixels(pixels);
            resultTexture.Apply();

            return resultTexture;
        }
    }
}