﻿using System;
using MonoGame.Squid.Structs;

namespace MonoGame.Squid.Interfaces
{
    /// <summary>
    /// Interface ISquidRenderer
    /// </summary>
    public interface ISquidRenderer
    {
        /// <summary>
        /// Gets the size of the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <returns>Point.</returns>
        Point GetTextSize(string text, string font);

        /// <summary>
        /// Gets the size of the texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <returns>Point.</returns>
        Point GetTextureSize(string texture);

        /// <summary>
        /// Set the scissor rectangle
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        void Scissor(int x, int y, int width, int height);

        /// <summary>
        /// Draws a box.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        void DrawBox(int x, int y, int width, int height, int color);

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="font">The font.</param>
        /// <param name="color">The color.</param>
        void DrawText(string text, int x, int y, string font, int color);

        /// <summary>
        /// Draws the texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="source">The source.</param>
        /// <param name="color">The color.</param>
        void DrawTexture(string texture, int x, int y, int width, int height, Rectangle source, int color);

        /// <summary>
        /// Starts the batch.
        /// </summary>
        void StartBatch();

        /// <summary>
        /// Ends the batch.
        /// </summary>
        /// <param name="final">if set to <c>true</c> [final].</param>
        void EndBatch(bool final);
    }

    /// <summary>
    /// And empty implementation of the ISquidRenderer interface.
    /// This is the default value of Gui.Renderer.
    /// </summary>
    public sealed class NoRenderer : ISquidRenderer
    {
        public void StartBatch() { }

        public void EndBatch(bool final) { }

        public int GetTexture(string name) { return -1; }

        public Point GetTextSize(string text, string font) { return new Point(); }

        public Point GetTextureSize(string texture) { return new Point(); }

        public void Scissor(int x, int y, int width, int height) { }

        public void DrawBox(int x, int y, int width, int height, int color) { }

        public void DrawTexture(string texture, int x, int y, int width, int height, Rectangle source, int color) { }

        public void DrawText(string text, int x, int y, string font, int color) { }

        public void Dispose() { }
    }
}
